using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using KafkaCurator.Abstractions;
using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Abstractions.Extensions;

namespace KafkaCurator.Configuration
{
    public class ClusterConfigurationBuilder : IClusterConfigurationBuilder
    {
        public IDependencyConfigurator DependencyConfigurator { get; }

        private readonly List<TopicConfigurationBuilder> _topics = new();
        private readonly List<string> _topicFiles = new();
        
        private string _brokers;
        private string _name;
        private ITopicsFileSerializer _topicsFileSerializer = new DefaultTopicsFileSerializer();
        private Func<SecurityInformation> _securityInformationHandler;
        private Action<IChangesManagerConfigurationBuilder> _changesManagerAction;

        public ClusterConfigurationBuilder(IDependencyConfigurator dependencyConfigurator)
        {
            DependencyConfigurator = dependencyConfigurator;
        }

        public IClusterConfigurationBuilder WithBrokers(IEnumerable<string> brokers)
        {
            _brokers = string.Join(",", brokers);
            return this;
        }

        public IClusterConfigurationBuilder WithBrokers(string brokers)
        {
            _brokers = brokers;
            return this;
        }

        public IClusterConfigurationBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
        
        public IClusterConfigurationBuilder WithSecurityInformation(Action<SecurityInformation> handler)
        {
            // Uses a handler to avoid in-memory stored passwords for long periods
            _securityInformationHandler = () =>
            {
                var config = new SecurityInformation();
                handler(config);
                return config;
            };

            return this;
        }

        public IClusterConfigurationBuilder WithCustomTopicsFileSerializer(ITopicsFileSerializer serializer)
        {
            _topicsFileSerializer = serializer;
            return this;
        }

        public IClusterConfigurationBuilder ConfigureChangesManager(Action<IChangesManagerConfigurationBuilder> changesManager)
        {
            _changesManagerAction = changesManager;
            return this;
        }

        public IClusterConfigurationBuilder AddTopic(Action<ITopicConfigurationBuilder> topic)
        {
            var builder = new TopicConfigurationBuilder(DependencyConfigurator);

            topic(builder);
            
            _topics.Add(builder);

            return this;
        }

        public IClusterConfigurationBuilder AddTopicsJsonFile(string path)
        {
            _topicFiles.Add(path);
            return this;
        }

        public ClusterConfiguration Build(KafkaConfiguration kafkaConfiguration)
        {
            var clusterConfiguration =
                new ClusterConfiguration(kafkaConfiguration, _name, _brokers, _securityInformationHandler);
            
            clusterConfiguration.AddChangesManager(BuildChangesManager(clusterConfiguration));
            clusterConfiguration.AddTopics(_topics.Select(t => t.Build(clusterConfiguration)));
            
            foreach (var topicsFile in _topicFiles)
            {
                var result = _topicsFileSerializer.Deserialize(File.ReadAllBytes(topicsFile));
                clusterConfiguration.AddTopics(result.Topics);
            }
            
            return clusterConfiguration;
        }

        private ChangesManagerConfiguration BuildChangesManager(ClusterConfiguration clusterConfiguration)
        {
            var builder = new ChangesManagerManagerConfigurationBuilder(DependencyConfigurator);
            
            _changesManagerAction(builder);

            return builder.Build(clusterConfiguration);
        }
    }
}