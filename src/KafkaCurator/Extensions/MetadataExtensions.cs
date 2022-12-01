using Confluent.Kafka;
using KafkaCurator.Configuration;

namespace KafkaCurator.Extensions
{
    public static class MetadataExtensions
    {
        public static TopicConfiguration ToTopicConfiguration(this TopicMetadata topicMetadata)
        {
            return new TopicConfiguration(topicMetadata.Topic)
            {
                Partitions = topicMetadata.Partitions.Count,
            };
        }
    }
}