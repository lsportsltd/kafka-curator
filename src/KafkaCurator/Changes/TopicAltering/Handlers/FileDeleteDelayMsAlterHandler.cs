using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class FileDeleteDelayMsAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.FileDeleteDelayMs;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.FileDeleteDelayMs == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.FileDeleteDelayMs);

            if (topicState.FileDeleteDelayMs == topicConfig.FileDeleteDelayMs)
                return new AlterHandlerResult(false, ConfigEntry, topicState.FileDeleteDelayMs, topicConfig.FileDeleteDelayMs);

            return new AlterHandlerResult(true, ConfigEntry, topicState.FileDeleteDelayMs, topicConfig.FileDeleteDelayMs);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.FileDeleteDelayMs = topicState.FileDeleteDelayMs;
        }
    }
}