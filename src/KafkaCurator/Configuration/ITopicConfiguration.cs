using KafkaCurator.Abstractions;

namespace KafkaCurator.Configuration
{
    public interface ITopicConfiguration
    {
        string Name { get; }
        int? Partitions { get; set; }
        short? ReplicationFactor { get; set; }
        string CleanupPolicy { get; set; }
        string CompressionType { get; set; }
        string MessageDownConversionEnable { get; set; }
        string MinInSyncReplicas { get; set; }
        string SegmentJitterMs { get; set; }
        string FlushMs { get; set; }
        string FollowerReplicationThrottledReplicas { get; set; }
        string SegmentBytes { get; set; }
        string RetentionMs { get; set; }
        string FlushMessages { get; set; }
        string MessageFormatVersion { get; set; }
        string MaxCompactionLagMs { get; set; }
        string FileDeleteDelayMs { get; set; }
        string MaxMessageBytes { get; set; }
        string MinCompactionLagMs { get; set; }
        string MessageTimestampType { get; set; }
        string Preallocate { get; set; }
        string MinCleanableDirtyRation { get; set; }
        string IndexIntervalBytes { get; set; }
        string UncleanLeaderElectionEnable { get; set; }
        string RetentionBytes { get; set; }
        string DeleteRetentionMs { get; set; }
        string SegmentMs { get; set; }
        string MessageTimestampDifferenceMaxMs { get; set; }
        string SegmentIndexBytes { get; set; }

        void SanityCheck();
    }
}