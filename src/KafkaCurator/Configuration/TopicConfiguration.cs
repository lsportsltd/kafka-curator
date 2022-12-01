namespace KafkaCurator.Configuration
{
    public class TopicConfiguration : ITopicConfiguration
    {
        public string Name { get; }
        public int? Partitions { get; set; }
        public int? ReplicationFactor { get; set; }
        public string CleanupPolicy { get; set; }
        public string CompressionType { get; set; }

        public TopicConfiguration(string name)
        {
            Name = name;
        }
    }
}