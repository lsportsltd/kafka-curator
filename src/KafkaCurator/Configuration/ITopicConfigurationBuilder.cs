using KafkaCurator.Abstractions;
using KafkaCurator.Abstractions.Configuration;

namespace KafkaCurator.Configuration
{
    public interface ITopicConfigurationBuilder
    {
        IDependencyConfigurator DependencyConfigurator { get; }
        ITopicConfigurationBuilder Name(string topicName);
        ITopicConfigurationBuilder WithNumberOfPartitions(int numberOfPartitions);
        ITopicConfigurationBuilder WithReplicationFactory(int replicationFactor);
        ITopicConfigurationBuilder WithCleanupPolicy(CleanupPolicy policy);
        ITopicConfigurationBuilder WithCompression(CompressionType compression);
    }
}