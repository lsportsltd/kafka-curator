using System;
using KafkaCurator.Abstractions;
using KafkaCurator.Abstractions.Configuration;

namespace KafkaCurator.Configuration
{
    public class TopicConfigurationBuilder : ITopicConfigurationBuilder
    {
        public IDependencyConfigurator DependencyConfigurator { get; }

        private string _topicName;
        private int? _numberOfPartitions;
        private int? _replicationFactor;
        private CleanupPolicy _cleanupPolicy;
        private CompressionType _compressionType;

        public TopicConfigurationBuilder(IDependencyConfigurator dependencyConfigurator)
        {
            DependencyConfigurator = dependencyConfigurator;
        }

        public ITopicConfigurationBuilder Name(string topicName)
        {
            _topicName = topicName;
            return this;
        }

        public ITopicConfigurationBuilder WithNumberOfPartitions(int numberOfPartitions)
        {
            _numberOfPartitions = numberOfPartitions;
            return this;
        }

        public ITopicConfigurationBuilder WithReplicationFactory(int replicationFactor)
        {
            _replicationFactor = replicationFactor;
            return this;
        }

        public ITopicConfigurationBuilder WithCleanupPolicy(CleanupPolicy policy)
        {
            _cleanupPolicy = policy;
            return this;
        }

        public ITopicConfigurationBuilder WithCompression(CompressionType compression)
        {
            _compressionType = compression;
            return this;
        }

        public ITopicConfiguration Build(ClusterConfiguration clusterConfiguration)
        {
            SanityCheck();
            
            return new TopicConfiguration(_topicName)
            {
                Partitions = _numberOfPartitions,
                ReplicationFactor = _replicationFactor,
                CleanupPolicy = _cleanupPolicy?.ToString(),
                CompressionType = _compressionType?.ToString()
            };
        }

        private void SanityCheck()
        {
            if (string.IsNullOrEmpty(_topicName)) throw new ArgumentNullException(nameof(_topicName));
            if (_numberOfPartitions <= 0) throw new ArgumentOutOfRangeException(nameof(_numberOfPartitions));
        }
    }
}