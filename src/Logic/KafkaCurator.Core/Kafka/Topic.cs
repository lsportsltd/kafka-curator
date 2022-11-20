using KafkaCurator.Core.Enums;

namespace KafkaCurator.Core.Kafka
{
    public class Topic
    {
        public string Name { get; set; }
        public short ReplicationFactor { get; set; }
        public int NumOfPartitions { get; set; }
        public CleanupPolicy CleanupPolicy { get; set; }
        public CompressionType? CompressionType { get; set; }
    }
}