using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka.Admin;
using KafkaCurator.Abstractions;
using KafkaCurator.Configuration;
using ConfigEntry = KafkaCurator.Abstractions.Configuration.ConfigEntry;

namespace KafkaCurator.Changes
{
    public class NewTopicsHandler : TopicsHandlerBase, INewTopicsHandler
    {
        string INewTopicsHandler.Name => _configuration.ClusterName;
        
        private readonly ChangesManagerConfiguration _configuration;
        private readonly ILogHandler _logHandler;

        public NewTopicsHandler(IDependencyResolver resolver, ChangesManagerConfiguration configuration) : base(resolver, configuration)
        {
            _configuration = configuration;
            _logHandler = resolver.Resolve<ILogHandler>();
        }

        public int PreviewNewTopics(IReadOnlyCollection<ITopicConfiguration> newTopics)
        {
            try
            {
                var topicsToCreate = GetTopicsToCreate(newTopics);
                return topicsToCreate.Count;
            }
            catch (Exception e)
            {
                _logHandler.Error("An error has occurred while previewing new topics.", e, null);
            }

            return 0;
        }

        public async Task<int> HandleNewTopics(IReadOnlyCollection<ITopicConfiguration> newTopics)
        {
            try
            {
                var topicsToCreate = GetTopicsToCreate(newTopics);
                if (topicsToCreate.Count == 0) return 0;
                
                await AdminClient.CreateTopicsAsync(topicsToCreate);
                return topicsToCreate.Count;
            }
            catch (Exception e)
            {
                _logHandler.Error("An error has occurred while handling new topics.", e, null);
            }

            return 0;
        }

        private List<TopicSpecification> GetTopicsToCreate(IReadOnlyCollection<ITopicConfiguration> newTopics)
        {
            var topicsToCreate = new List<TopicSpecification>();

            if (newTopics.Count == 0)
            {
                _logHandler.Info("    There are no new topics to create.");
                return topicsToCreate;
            }

            foreach (var topicConfiguration in newTopics)
            {
                topicConfiguration.SanityCheck();

                var topicSpecification = new TopicSpecification
                {
                    Name = topicConfiguration.Name,
                    NumPartitions = topicConfiguration.Partitions!.Value,
                    ReplicationFactor = topicConfiguration.ReplicationFactor!.Value,
                    Configs = GetNewTopicConfigs(topicConfiguration)
                };

                topicsToCreate.Add(topicSpecification);
                PrintNewTopic(topicConfiguration);
            }

            return topicsToCreate;
        }
        
        private Dictionary<string, string> GetNewTopicConfigs(ITopicConfiguration topicConfiguration)
        {
            var topicConfigs = new Dictionary<string, string>();
            var configEntries = Enumeration.GetAll<ConfigEntry>().ToArray();
            var properties = topicConfiguration.GetType().GetProperties().ToDictionary(p => p.Name, p => p);

            foreach (var configEntry in configEntries)
            {
                if (!properties.TryGetValue(configEntry.EntryName, out var prop)) continue;

                var propValue = prop.GetValue(topicConfiguration) as string;
                if (string.IsNullOrEmpty(propValue)) continue;

                topicConfigs[configEntry.ToString()] = propValue;
            }

            return topicConfigs;
        }

        private void PrintNewTopic(ITopicConfiguration topicConfiguration)
        {
            var properties = topicConfiguration.GetType().GetProperties();

            var firstProperty = true;
            foreach (var property in properties)
            {
                var propValue = property.GetValue(topicConfiguration)?.ToString();
                if(string.IsNullOrEmpty(propValue)) continue;

                if (firstProperty)
                {
                    _logHandler.Info($"  + {property.Name}: {propValue}");
                    firstProperty = false;
                }
                else
                {
                    var indent = new string(' ', (1 * 4));
                    _logHandler.Info($"{indent}{property.Name}: {propValue}");
                }
            }
        }
    }
}