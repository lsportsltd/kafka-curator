using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering
{
    public interface ITopicAlterHandler
    {
        ConfigEntry ConfigEntry { get; }
        AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState);
        void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState);
    }
}