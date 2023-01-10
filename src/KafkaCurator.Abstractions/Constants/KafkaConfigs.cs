namespace KafkaCurator.Abstractions.Constants
{
    public static class KafkaConfigs
    {
        public const string CleanupPolicy = "cleanup.policy";
        public const string CompressionType = "compression.type";
        public const string MessageDownConversionEnable = "message.downconversion.enable";
        public const string MinInSyncReplicas = "min.insync.replicas";
        public const string SegmentJitterMs = "segment.jitter.ms";
        public const string FlushMs = "flush.ms";
        public const string FollowerReplicationThrottledReplicas = "follower.replication.throttled.replicas";
        public const string SegmentBytes = "segment.bytes";
        public const string RetentionMs = "retention.ms";
        public const string FlushMessages = "flush.messages";
        public const string MessageFormatVersion = "message.format.version";
        public const string MaxCompactionLagMs = "max.compaction.lag.ms";
        public const string FileDeleteDelayMs = "file.delete.delay.ms";
        public const string MaxMessageBytes = "max.message.bytes";
        public const string MinCompactionLagMs = "min.compaction.lag.ms";
        public const string MessageTimestampType = "message.timestamp.type";
        public const string Preallocate = "preallocate";
        public const string MinCleanableDirtyRation = "min.cleanable.dirty.ratio";
        public const string IndexIntervalBytes = "index.interval.bytes";
        public const string UncleanLeaderElectionEnable = "unclean.leader.election.enable";
        public const string RetentionBytes = "retention.bytes";
        public const string DeleteRetentionMs = "delete.retention.ms";
        public const string SegmentMs = "segment.ms";
        public const string MessageTimestampDifferenceMaxMs = "message.timestamp.difference.max.ms";
        public const string SegmentIndexBytes = "segment.index.bytes";
    }
}