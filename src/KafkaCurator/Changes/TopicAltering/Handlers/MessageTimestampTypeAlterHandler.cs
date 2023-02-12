using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class MessageTimestampTypeAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.MessageTimestampType;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.MessageTimestampType == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.MessageTimestampType);

            if (topicState.MessageTimestampType == topicConfig.MessageTimestampType)
                return new AlterHandlerResult(false, ConfigEntry, topicState.MessageTimestampType, topicConfig.MessageTimestampType);

            return new AlterHandlerResult(true, ConfigEntry, topicState.MessageTimestampType, topicConfig.MessageTimestampType);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.MessageTimestampType = topicState.MessageTimestampType;
        }
    }
}