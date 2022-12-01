namespace KafkaCurator.Configuration
{
    public interface ITopicConfiguration
    {
        string Name { get; }
        int? Partitions { get; set; }
        int? ReplicationFactor { get; set; }
        string CleanupPolicy { get; set; }
        string CompressionType { get; set; }
    }
}