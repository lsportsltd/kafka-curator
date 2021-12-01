using KafkaCurator.Core.Enums;

namespace KafkaCurator.Models
{
    public class Topic
    {
        public string Name { get; set; }
        public short ReplicationFactor { get; set; }
        public int NumOfPartitions { get; set; }
        public CleanupPolicy CleanupPolicy { get; set; }
    }
}