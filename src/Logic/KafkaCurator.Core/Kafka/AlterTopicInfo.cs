namespace KafkaCurator.Core.Kafka
{
    public class AlterTopicInfo
    {
        public bool ShouldAlterNumOfPartitions { get; set; } = false;
        public bool ShouldAlterCleanupPolicy { get; set; } = false;
        public bool ShouldAlterCompression { get; set; } = false;
    }
}