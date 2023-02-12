using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka.Admin;
using KafkaCurator.Abstractions;
using KafkaCurator.Changes.TopicAltering;
using KafkaCurator.Configuration;
using ConfigEntry = KafkaCurator.Abstractions.Configuration.ConfigEntry;

namespace KafkaCurator.Changes
{
    public class ExistingTopicsHandler : TopicsHandlerBase, IExistingTopicsHandler
    {
        string IExistingTopicsHandler.Name => _configuration.ClusterName;

        private readonly ChangesManagerConfiguration _configuration;
        private readonly ILogHandler _logHandler;
        private readonly ITopicAlterFactory _altersFactory;

        public ExistingTopicsHandler(IDependencyResolver dependencyResolver, ChangesManagerConfiguration configuration) : base(dependencyResolver, configuration)
        {
            _configuration = configuration;
            _logHandler = DependencyResolver.Resolve<ILogHandler>();
            _altersFactory = DependencyResolver.Resolve<ITopicAlterFactory>();
        }

        public int PreviewExistingTopics((ITopicConfiguration, ITopicConfiguration)[] topicPairs)
        {
            try
            {
                if (InitialSanityCheck(topicPairs) == false) return 0;

                var (_, _, numberOfUpdates) = PreviewChangesInternal(topicPairs);
                return numberOfUpdates;
            }
            catch (Exception e)
            {
                _logHandler.Error("An error has occurred while previewing existing topics.", e, null);
            }

            return 0;
        }

        public async Task<int> HandleExistingTopics((ITopicConfiguration, ITopicConfiguration)[] topicPairs)
        {
            try
            {
                if (InitialSanityCheck(topicPairs) == false) return 0;

                var (configEntriesToAlter, partitionsSpecifications, numberOfUpdates) = PreviewChangesInternal(topicPairs);

                await HandleChangesInternal(configEntriesToAlter, partitionsSpecifications);
                return numberOfUpdates;
            }
            catch (Exception e)
            {
                _logHandler.Error("An error has occurred while handling existing topics.", e, null);
            }

            return 0;
        }

        private (List<Dictionary<ConfigResource, List<Confluent.Kafka.Admin.ConfigEntry>>> topicConfigChanges, List<PartitionsSpecification> partitionsSpecifications, int)
            PreviewChangesInternal((ITopicConfiguration, ITopicConfiguration)[] topicPairs)
        {
            var numberOfUpdates = 0;

            var configEntries = Enumeration.GetAll<ConfigEntry>().ToArray();
            var partitionsSpecifications = new List<PartitionsSpecification>();
            var configEntriesToAlter = new List<Dictionary<ConfigResource, List<Confluent.Kafka.Admin.ConfigEntry>>>();

            foreach (var (configTopic, stateTopic) in topicPairs)
            {
                if (configTopic.Partitions < stateTopic.Partitions)
                {
                    throw new ArgumentException(
                        "The number of partitions for a topic cannot be decreased, in order to increase, re-add the topic.");
                }

                var (configEntriesResult, diffs) = GetConfigEntriesToAlter(configTopic, stateTopic, configEntries);
                if (configEntriesResult != null)
                {
                    configEntriesToAlter.Add(configEntriesResult);
                }

                //numberOfUpdates++;

                var partitionSpecification = GetPartitionSpecification(configTopic, stateTopic);
                if (partitionSpecification != null)
                {
                    partitionsSpecifications.Add(partitionSpecification);
                    diffs.Add("partitions");
                }

                if (diffs.Count > 0) numberOfUpdates++;
                
                LogChanges(diffs, configTopic.Name);
            }

            return (configEntriesToAlter, partitionsSpecifications, numberOfUpdates);
        }

        private async Task HandleChangesInternal(List<Dictionary<ConfigResource, List<Confluent.Kafka.Admin.ConfigEntry>>> topicConfigChanges,
            List<PartitionsSpecification> partitionsSpecifications)
        {
            await HandleTopicConfigChanges(topicConfigChanges);
            await HandleTopicPartitionChanges(partitionsSpecifications);
        }

        private async Task HandleTopicConfigChanges(List<Dictionary<ConfigResource, List<Confluent.Kafka.Admin.ConfigEntry>>> topicConfigChanges)
        {
            foreach (var topicConfigChange in topicConfigChanges)
            {
                await AdminClient.AlterConfigsAsync(topicConfigChange,
                    new AlterConfigsOptions {RequestTimeout = _configuration.Timeout});
            }
        }

        private Task HandleTopicPartitionChanges(List<PartitionsSpecification> partitionsSpecifications)
        {
            if (ShouldCreateNewPartitions(partitionsSpecifications) == false) return Task.CompletedTask;

            return AdminClient.CreatePartitionsAsync(partitionsSpecifications);
        }

        private (Dictionary<ConfigResource, List<Confluent.Kafka.Admin.ConfigEntry>>, List<string>) GetConfigEntriesToAlter(ITopicConfiguration configTopic,
            ITopicConfiguration stateTopic,
            ConfigEntry[] configEntries)
        {
            var configsToAlter = new List<AlterHandlerResult>();

            foreach (var configEntry in configEntries)
            {
                var handler = _altersFactory.GetHandler(configEntry.Name);
                if (handler == null) continue;

                var result = handler.ShouldAlter(configTopic, stateTopic);
                if (!result.ShouldAlter && result.OldValue == null) continue;

                configsToAlter.Add(result);
            }

            if (configsToAlter.Count == 0) return (null, null);

            var diffs = GetConfigEntryDiffs(configsToAlter);

            var configResource = new ConfigResource
            {
                Name = configTopic.Name,
                Type = ResourceType.Topic
            };

            var configs = configsToAlter.Select(x => new Confluent.Kafka.Admin.ConfigEntry
                {Name = x.ConfigEntry.ToString(), Value = x.NewValue}).ToList();

            var kafkaTopicConfigsDictionary =
                new Dictionary<ConfigResource, List<Confluent.Kafka.Admin.ConfigEntry>>
                {
                    {configResource, configs}
                };

            return (kafkaTopicConfigsDictionary, diffs);
        }

        private List<string> GetConfigEntryDiffs(List<AlterHandlerResult> configsToAlter)
        {
            var diff = new List<string>();

            foreach (var alterHandlerResult in configsToAlter)
            {
                if (!alterHandlerResult.ShouldAlter) continue;

                if (alterHandlerResult.OldValue == null)
                {
                    diff.Add(alterHandlerResult.ConfigEntry.Name);
                }
                else if (alterHandlerResult.NewValue != null && alterHandlerResult.OldValue != null)
                {
                    diff.Add(alterHandlerResult.ConfigEntry.Name);
                }
            }

            return diff;
        }

        private void LogChanges(IReadOnlyList<string> diffs, string topicName)
        {
            if (diffs.Count > 0)
            {
                _logHandler.Info($" ~  topic: {topicName} [diff: {string.Join(", ", diffs)}].");
            }
            else
            {
                _logHandler.Info($"    topic: {topicName}");
            }
        }

        private PartitionsSpecification GetPartitionSpecification(ITopicConfiguration configTopic,
            ITopicConfiguration topicState)
        {
            if (configTopic.Partitions == null || configTopic.Partitions == topicState.Partitions) return null;

            var partitionSpecification = new PartitionsSpecification
            {
                IncreaseTo = configTopic.Partitions.Value,
                Topic = configTopic.Name
            };

            return partitionSpecification;
        }

        private bool ShouldCreateNewPartitions(List<PartitionsSpecification> partitionsSpecifications) =>
            partitionsSpecifications.Count != 0;

        private bool InitialSanityCheck((ITopicConfiguration, ITopicConfiguration)[] topicPairs)
        {
            if (topicPairs.Length == 0)
            {
                _logHandler.Info("    there are no topics to alter.");
                return false;
            }

            return true;
        }
    }
}