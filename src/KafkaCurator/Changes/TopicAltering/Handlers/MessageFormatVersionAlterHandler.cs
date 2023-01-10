using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class MessageFormatVersionAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.MessageFormatVersion;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.MessageFormatVersion == null || topicConfig.MessageFormatVersion == topicState.MessageFormatVersion)
                return new AlterHandlerResult();

            return new AlterHandlerResult(true, ConfigEntry, topicConfig.MessageFormatVersion);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.MessageFormatVersion = topicState.MessageFormatVersion;
        }
    }
}