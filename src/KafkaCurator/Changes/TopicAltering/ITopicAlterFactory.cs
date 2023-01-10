using Confluent.Kafka;

namespace KafkaCurator.Changes.TopicAltering
{
    public interface ITopicAlterFactory
    {
        ITopicAlterHandler GetHandler(string configEntry);
    }
}