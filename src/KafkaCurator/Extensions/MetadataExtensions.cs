using System.Collections.Generic;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaCurator.Abstractions.Constants;
using KafkaCurator.Configuration;

namespace KafkaCurator.Extensions
{
    public static class MetadataExtensions
    {
        public static ITopicConfiguration ToTopicConfiguration(this TopicMetadata topicMetadata)
        {
            return new TopicConfiguration(topicMetadata.Topic)
            {
                Partitions = topicMetadata.Partitions.Count,
            };
        }
        
        public static ITopicConfiguration ToTopicConfiguration(this TopicMetadata topicMetadata, DescribeConfigsResult describeConfig)
        {
            return new TopicConfiguration(topicMetadata.Topic)
            {
                Partitions = topicMetadata.Partitions.Count,
                CleanupPolicy = GetConfigValue(describeConfig.Entries, KafkaConfigs.CleanupPolicy),
                CompressionType = GetConfigValue(describeConfig.Entries, KafkaConfigs.CompressionType),
                Preallocate = GetConfigValue(describeConfig.Entries, KafkaConfigs.Preallocate),
                FlushMessages = GetConfigValue(describeConfig.Entries, KafkaConfigs.FlushMessages),
                FlushMs = GetConfigValue(describeConfig.Entries, KafkaConfigs.FlushMs),
                RetentionBytes = GetConfigValue(describeConfig.Entries, KafkaConfigs.RetentionBytes),
                RetentionMs = GetConfigValue(describeConfig.Entries, KafkaConfigs.RetentionMs),
                SegmentBytes = GetConfigValue(describeConfig.Entries, KafkaConfigs.SegmentBytes),
                SegmentMs = GetConfigValue(describeConfig.Entries, KafkaConfigs.SegmentMs),
                DeleteRetentionMs = GetConfigValue(describeConfig.Entries, KafkaConfigs.DeleteRetentionMs),
                IndexIntervalBytes = GetConfigValue(describeConfig.Entries, KafkaConfigs.IndexIntervalBytes),
                MaxMessageBytes = GetConfigValue(describeConfig.Entries, KafkaConfigs.MaxMessageBytes),
                MessageFormatVersion = GetConfigValue(describeConfig.Entries, KafkaConfigs.MessageFormatVersion),
                MessageTimestampType = GetConfigValue(describeConfig.Entries, KafkaConfigs.MessageTimestampType),
                SegmentIndexBytes = GetConfigValue(describeConfig.Entries, KafkaConfigs.SegmentIndexBytes),
                SegmentJitterMs = GetConfigValue(describeConfig.Entries, KafkaConfigs.SegmentJitterMs),
                FileDeleteDelayMs = GetConfigValue(describeConfig.Entries, KafkaConfigs.FileDeleteDelayMs),
                FollowerReplicationThrottledReplicas = GetConfigValue(describeConfig.Entries, KafkaConfigs.FollowerReplicationThrottledReplicas),
                MaxCompactionLagMs = GetConfigValue(describeConfig.Entries, KafkaConfigs.MaxCompactionLagMs),
                MessageDownConversionEnable = GetConfigValue(describeConfig.Entries, KafkaConfigs.MessageDownConversionEnable),
                MinCleanableDirtyRation = GetConfigValue(describeConfig.Entries, KafkaConfigs.MinCleanableDirtyRation),
                MinCompactionLagMs = GetConfigValue(describeConfig.Entries, KafkaConfigs.MinCompactionLagMs),
                MinInSyncReplicas = GetConfigValue(describeConfig.Entries, KafkaConfigs.MinInSyncReplicas),
                UncleanLeaderElectionEnable = GetConfigValue(describeConfig.Entries, KafkaConfigs.UncleanLeaderElectionEnable),
                MessageTimestampDifferenceMaxMs = GetConfigValue(describeConfig.Entries, KafkaConfigs.MessageTimestampDifferenceMaxMs)
            };
        }

        private static string GetConfigValue(Dictionary<string, ConfigEntryResult> entries, string key)
        => entries.TryGetValue(key, out var result) ? result.Value : null;
    }
}