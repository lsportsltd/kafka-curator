using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaCurator.Abstractions;
using KafkaCurator.Changes.TopicAltering;
using KafkaCurator.Configuration;
using KafkaCurator.State;
using ConfigEntry = KafkaCurator.Abstractions.Configuration.ConfigEntry;

namespace KafkaCurator.Changes
{
    internal class ChangesManager : IChangesManager
    {
        public string Name => _changesManagerConfiguration.ClusterName;

        private readonly ILogHandler _logHandler;
        private readonly ChangesManagerConfiguration _changesManagerConfiguration;
        private readonly ITopicAlterFactory _altersFactory;

        private readonly IAdminClient _adminClient;
        private readonly IStateManager _stateManager;

        public ChangesManager(IDependencyResolver dependencyResolver,
            ChangesManagerConfiguration changesManagerConfiguration)
        {
            _logHandler = dependencyResolver.Resolve<ILogHandler>();
            _changesManagerConfiguration = changesManagerConfiguration;
            _altersFactory = dependencyResolver.Resolve<ITopicAlterFactory>();

            _adminClient = GetAdminClient(dependencyResolver, changesManagerConfiguration);
            _stateManager = GetStateManager(dependencyResolver, changesManagerConfiguration);
        }

        public async Task HandleChanges(IReadOnlyList<ITopicConfiguration> topics)
        {
            var state = (await _stateManager.GetState(new EmptyStateParameters())).Where(x =>
                    _changesManagerConfiguration.PrefixesToExclude.All(prefix => x.Name.StartsWith(prefix) == false))
                .ToDictionary(t => t.Name, t => t);

            _logHandler.Info($"Found {topics.Count} topics within configuration...");
            _logHandler.Info($"Found {state.Count} topics within state...");

            _logHandler.Info("Handling existing topics...");
            var existingTopics = Intersect(topics, state).ToArray();
            await HandleExistingTopics(existingTopics);
            _logHandler.Info("Done.");

            _logHandler.Info("Handling new topics...");
            var newTopics = topics.Where(t => !state.ContainsKey(t.Name)).ToArray();
            await HandleNewTopics(newTopics);
            _logHandler.Info("Done.");

            _logHandler.Info("Handling topics to delete...");
            var topicsToDelete = state.Keys.Except(topics.Select(t => t.Name)).ToArray();
            await HandleTopicsToDelete(topicsToDelete);
            _logHandler.Info("Done.");
            
            _logHandler.Info("Updating state...");
            await _stateManager.SetState(topics);
            _logHandler.Info("Done.");
        }

        private async Task HandleExistingTopics((ITopicConfiguration, ITopicConfiguration)[] topicPairs)
        {
            try
            {
                if (topicPairs.Length == 0)
                {
                    _logHandler.Info("There are no topics to alter.");
                    return;
                }

                var configEntries = Enumeration.GetAll<ConfigEntry>().ToArray();
                var partitionsSpecifications = new List<PartitionsSpecification>();

                foreach (var (configTopic, stateTopic) in topicPairs)
                {
                    if (configTopic.Partitions < stateTopic.Partitions)
                    {
                        throw new ArgumentException(
                            "The number of partitions for a topic cannot be decreased, in order to increase, re-add the topic.");
                    }

                    await HandleConfigEntriesAlteration(configTopic, stateTopic, configEntries);
                    HandleRelevantPartitionSpecification(configTopic, stateTopic, partitionsSpecifications);
                }

                if (ShouldCreateNewPartitions(partitionsSpecifications) == false) return;

                foreach (var partitionsSpecification in partitionsSpecifications)
                {
                    _logHandler.Info(
                        $"Altering partitions for topic: {partitionsSpecification.Topic}, new value: {partitionsSpecification.IncreaseTo}");
                }

                await _adminClient.CreatePartitionsAsync(partitionsSpecifications);
            }
            catch (Exception e)
            {
                _logHandler.Error("An error has occurred while handling existing topics.", e, null);
            }
        }

        private Task HandleTopicsToDelete(string[] topics)
        {
            try
            {
                if (topics.Length == 0)
                {
                    _logHandler.Info("There are no topics to delete.");
                    return Task.CompletedTask;
                }

                _logHandler.Info($"Deleting the following topics: {string.Join(",", topics)}");
                return _adminClient.DeleteTopicsAsync(topics);
            }
            catch (Exception e)
            {
                _logHandler.Error("An error has occurred while handling topics to delete.", e, null);
                return Task.CompletedTask;
            }
        }

        private async Task HandleNewTopics(IReadOnlyCollection<ITopicConfiguration> newTopics)
        {
            try
            {
                if (newTopics.Count == 0)
                {
                    _logHandler.Info("There are no new topics to create.");
                    return;
                }

                var topicsToCreate = new List<TopicSpecification>();
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
                    _logHandler.Info(topicConfiguration.ToString());
                }

                await _adminClient.CreateTopicsAsync(topicsToCreate);
            }
            catch (Exception e)
            {
                _logHandler.Error("An error has occurred while handling new topics.", e, null);
            }
        }

        private bool ShouldCreateNewPartitions(List<PartitionsSpecification> partitionsSpecifications) =>
            partitionsSpecifications.Count != 0;

        private void HandleRelevantPartitionSpecification(ITopicConfiguration configTopic,
            ITopicConfiguration topicState, List<PartitionsSpecification> partitionsSpecifications)
        {
            if (configTopic.Partitions == null || configTopic.Partitions == topicState.Partitions) return;

            var partitionSpecification = new PartitionsSpecification
            {
                IncreaseTo = configTopic.Partitions.Value,
                Topic = configTopic.Name
            };

            partitionsSpecifications.Add(partitionSpecification);
        }

        private async Task HandleConfigEntriesAlteration(ITopicConfiguration configTopic,
            ITopicConfiguration stateTopic,
            ConfigEntry[] configEntries)
        {
            var configsToAlter = new List<AlterHandlerResult>();

            foreach (var configEntry in configEntries)
            {
                var handler = _altersFactory.GetHandler(configEntry.Name);
                if (handler == null) continue;

                var result = handler.ShouldAlter(configTopic, stateTopic);
                if (!result.ShouldAlter) continue;

                configsToAlter.Add(result);
            }

            if (configsToAlter.Count == 0) return;

            _logHandler.Info($"Altering configs for topic {configTopic.Name}.");
            foreach (var alterHandlerResult in configsToAlter)
            {
                _logHandler.Info(
                    $"Config entry: {alterHandlerResult.ConfigEntry.EntryName}, new value: {alterHandlerResult.NewValue}");
            }

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

            await _adminClient.AlterConfigsAsync(kafkaTopicConfigsDictionary,
                new AlterConfigsOptions {RequestTimeout = _changesManagerConfiguration.Timeout});
        }

        private IAdminClient GetAdminClient(IDependencyResolver dependencyResolver,
            ChangesManagerConfiguration changesManagerConfiguration)
        {
            var adminClientFactory = dependencyResolver.Resolve<IAdminClientFactory>();

            return adminClientFactory.GetOrCreate(changesManagerConfiguration.ClusterName,
                changesManagerConfiguration.AdminClientConfig);
        }

        private IStateManager GetStateManager(IDependencyResolver dependencyResolver,
            ChangesManagerConfiguration changesManagerConfiguration)
        {
            var adminClientFactory = dependencyResolver.Resolve<IAdminClientFactory>();
            var adminClient = adminClientFactory.GetOrCreate(changesManagerConfiguration.ClusterName,
                changesManagerConfiguration.AdminClientConfig);

            if (changesManagerConfiguration.StateManagerConfiguration.Type == null)
            {
                return new DefaultStateManager(adminClient, changesManagerConfiguration.StateManagerConfiguration);
            }

            return dependencyResolver.Resolve(changesManagerConfiguration.StateManagerConfiguration.Type) as
                IStateManager;
        }

        private IEnumerable<(ITopicConfiguration, ITopicConfiguration)> Intersect(
            IReadOnlyList<ITopicConfiguration> topics, IReadOnlyDictionary<string, ITopicConfiguration> state)
        {
            foreach (var topicConfiguration in topics)
            {
                if (state.TryGetValue(topicConfiguration.Name, out var topic)) yield return (topicConfiguration, topic);
            }
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
    }
}