using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KafkaCurator.Configuration
{
    public class TopicConfiguration : ITopicConfiguration
    {
        public string Name { get; }
        public int? Partitions { get; set; }
        public short? ReplicationFactor { get; set; }
        public string CleanupPolicy { get; set; }
        public string CompressionType { get; set; }
        public string MessageDownConversionEnable { get; set; }
        public string MinInSyncReplicas { get; set; }
        public string SegmentJitterMs { get; set; }
        public string FlushMs { get; set; }
        public string FollowerReplicationThrottledReplicas { get; set; }
        public string SegmentBytes { get; set; }
        public string RetentionMs { get; set; }
        public string FlushMessages { get; set; }
        public string MessageFormatVersion { get; set; }
        public string MaxCompactionLagMs { get; set; }
        public string FileDeleteDelayMs { get; set; }
        public string MaxMessageBytes { get; set; }
        public string MinCompactionLagMs { get; set; }
        public string MessageTimestampType { get; set; }
        public string Preallocate { get; set; }
        public string MinCleanableDirtyRation { get; set; }
        public string IndexIntervalBytes { get; set; }
        public string UncleanLeaderElectionEnable { get; set; }
        public string RetentionBytes { get; set; }
        public string DeleteRetentionMs { get; set; }
        public string SegmentMs { get; set; }
        public string MessageTimestampDifferenceMaxMs { get; set; }
        public string SegmentIndexBytes { get; set; }
        
        public TopicConfiguration(string name)
        {
            Name = name;
        }
        
        public void SanityCheck()
        {
            if (Partitions == null) throw new ArgumentNullException(nameof(Partitions));
            if (ReplicationFactor == null) throw new ArgumentNullException(nameof(ReplicationFactor));
        }

        public override string ToString()
        {
            var properties = this.GetType().GetProperties();
            
            var stringBuilder = new StringBuilder();
            foreach (var property in properties)
            {
                var propValue = property.GetValue(this)?.ToString();
                if(string.IsNullOrEmpty(propValue)) continue;
                stringBuilder.AppendLine($"{property.Name}: {propValue}");
            }

            return stringBuilder.ToString();
        }
    }
}