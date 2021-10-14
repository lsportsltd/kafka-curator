namespace KafkaCurator.Models
{
    public class Topic
    {
        public string Name { get; set; }
        public short ReplicationFactor { get; set; }
        public int NumOfPartitions { get; set; }
    }
}