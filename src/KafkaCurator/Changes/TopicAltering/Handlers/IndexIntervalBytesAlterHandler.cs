using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class IndexIntervalBytesAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.IndexIntervalBytes;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.IndexIntervalBytes == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.IndexIntervalBytes);

            if (topicState.IndexIntervalBytes == topicConfig.IndexIntervalBytes)
                return new AlterHandlerResult(false, ConfigEntry, topicState.IndexIntervalBytes, topicConfig.IndexIntervalBytes);

            return new AlterHandlerResult(true, ConfigEntry, topicState.IndexIntervalBytes, topicConfig.IndexIntervalBytes);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.IndexIntervalBytes = topicState.IndexIntervalBytes;
        }
    }
}