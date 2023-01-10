using KafkaCurator.Abstractions;
using KafkaCurator.Abstractions.Configuration;

namespace KafkaCurator.Configuration
{
    public interface ITopicConfigurationBuilder
    {
        IDependencyConfigurator DependencyConfigurator { get; }
        ITopicConfigurationBuilder Name(string topicName);
        ITopicConfigurationBuilder WithNumberOfPartitions(int numberOfPartitions);
        ITopicConfigurationBuilder WithReplicationFactory(short replicationFactor);
        ITopicConfigurationBuilder WithCleanupPolicy(string cleanupPolicy);
        ITopicConfigurationBuilder WithCompression(string compression);
        ITopicConfigurationBuilder WithMessageDownConversionEnable(bool enable);
        ITopicConfigurationBuilder WithMinInSyncReplicas(int inSyncReplicas);
        ITopicConfigurationBuilder WithSegmentJitterMs(long segmentJitterMs);
        ITopicConfigurationBuilder WithFlushMs(long flushMs);
        ITopicConfigurationBuilder WithFollowerReplicationThrottledReplicas(string list);
        ITopicConfigurationBuilder WithSegmentBytes(int segmentBytes);
        ITopicConfigurationBuilder WithRetentionMs(long retentionMs);
        ITopicConfigurationBuilder WithFlushMessages(long flushMessages);
        ITopicConfigurationBuilder WithMessageFormatVersion(string messageFormatVersion);
        ITopicConfigurationBuilder WithMaxCompactionLagMs(long maxCompactionLagMs);
        ITopicConfigurationBuilder WithFileDeleteDelayMs(long fileDeleteDelayMs);
        ITopicConfigurationBuilder WithMaxMessageBytes(int maxMessageBytes);
        ITopicConfigurationBuilder WithMinCompactionLagMs(long minCompactionLagMs);
        ITopicConfigurationBuilder WithMessageTimestampType(string messageTimestampType);
        ITopicConfigurationBuilder WithPreallocate(bool preallocate);
        ITopicConfigurationBuilder WithMinCleanableDirtyRation(double minCleanableDirtyRation);
        ITopicConfigurationBuilder WithIndexIntervalBytes(int indexIntervalBytes);
        ITopicConfigurationBuilder WithUncleanLeaderElectionEnable(bool uncleanLeaderElectionEnable);
        ITopicConfigurationBuilder WithRetentionBytes(long retentionBytes);
        ITopicConfigurationBuilder WithDeleteRetentionMs(long deleteRetentionMs);
        ITopicConfigurationBuilder WithSegmentMs(long segmentMs);
        ITopicConfigurationBuilder WithMessageTimestampDifferenceMaxMs(long messageTimestampDifferenceMaxMs);
        ITopicConfigurationBuilder WithSegmentIndexBytes(int segmentIndexBytes);
    }
}