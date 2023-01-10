using KafkaCurator.Abstractions.Constants;

namespace KafkaCurator.Abstractions.Configuration
{
    public class ConfigEntry : Enumeration
    {
        public static ConfigEntry CleanupPolicy = new ConfigEntry(nameof(CleanupPolicy), KafkaConfigs.CleanupPolicy);
        public static ConfigEntry CompressionType = new ConfigEntry(nameof(CompressionType), KafkaConfigs.CompressionType);
        public static ConfigEntry MessageDownConversionEnable = new ConfigEntry(nameof(MessageDownConversionEnable),KafkaConfigs.MessageDownConversionEnable);
        public static ConfigEntry MinInSyncReplicas = new ConfigEntry(nameof(MinInSyncReplicas),KafkaConfigs.MinInSyncReplicas);
        public static ConfigEntry SegmentJitterMs = new ConfigEntry(nameof(SegmentJitterMs),KafkaConfigs.SegmentJitterMs);
        public static ConfigEntry FlushMs = new ConfigEntry(nameof(FlushMs),KafkaConfigs.FlushMs);
        public static ConfigEntry FollowerReplicationThrottledReplicas = new ConfigEntry(nameof(FollowerReplicationThrottledReplicas),KafkaConfigs.FollowerReplicationThrottledReplicas);
        public static ConfigEntry SegmentBytes = new ConfigEntry(nameof(SegmentBytes),KafkaConfigs.SegmentBytes);
        public static ConfigEntry RetentionMs = new ConfigEntry(nameof(RetentionMs),KafkaConfigs.RetentionMs);
        public static ConfigEntry FlushMessages = new ConfigEntry(nameof(FlushMessages),KafkaConfigs.FlushMessages);
        public static ConfigEntry MessageFormatVersion = new ConfigEntry(nameof(MessageFormatVersion),KafkaConfigs.MessageFormatVersion);
        public static ConfigEntry MaxCompactionLagMs = new ConfigEntry(nameof(MaxCompactionLagMs),KafkaConfigs.MaxCompactionLagMs);
        public static ConfigEntry MaxMessageBytes = new ConfigEntry(nameof(MaxMessageBytes),KafkaConfigs.MaxMessageBytes);
        public static ConfigEntry MinCompactionLagMs = new ConfigEntry(nameof(MinCompactionLagMs),KafkaConfigs.MinCompactionLagMs);
        public static ConfigEntry MessageTimestampType = new ConfigEntry(nameof(MessageTimestampType),KafkaConfigs.MessageTimestampType);
        public static ConfigEntry Preallocate = new ConfigEntry(nameof(Preallocate),KafkaConfigs.Preallocate);
        public static ConfigEntry MinCleanableDirtyRation = new ConfigEntry(nameof(MinCleanableDirtyRation),KafkaConfigs.MinCleanableDirtyRation);
        public static ConfigEntry IndexIntervalBytes = new ConfigEntry(nameof(IndexIntervalBytes),KafkaConfigs.IndexIntervalBytes);
        public static ConfigEntry UncleanLeaderElectionEnable = new ConfigEntry(nameof(UncleanLeaderElectionEnable),KafkaConfigs.UncleanLeaderElectionEnable);
        public static ConfigEntry RetentionBytes = new ConfigEntry(nameof(RetentionBytes),KafkaConfigs.RetentionBytes);
        public static ConfigEntry DeleteRetentionMs = new ConfigEntry(nameof(DeleteRetentionMs),KafkaConfigs.DeleteRetentionMs);
        public static ConfigEntry SegmentMs = new ConfigEntry(nameof(SegmentMs),KafkaConfigs.SegmentMs);
        public static ConfigEntry MessageTimestampDifferenceMaxMs = new ConfigEntry(nameof(MessageTimestampDifferenceMaxMs),KafkaConfigs.MessageTimestampDifferenceMaxMs);
        public static ConfigEntry SegmentIndexBytes = new ConfigEntry(nameof(SegmentIndexBytes),KafkaConfigs.SegmentIndexBytes);
        public static ConfigEntry FileDeleteDelayMs = new ConfigEntry(nameof(FileDeleteDelayMs),KafkaConfigs.MaxCompactionLagMs);
        
        public string EntryName { get; }
        
        public ConfigEntry(string entryName, string name) : base(name)
        {
            EntryName = entryName;
        }
    }
}