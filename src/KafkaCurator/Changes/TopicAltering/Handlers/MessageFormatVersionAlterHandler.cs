using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class MessageFormatVersionAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.MessageFormatVersion;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.MessageFormatVersion == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.MessageFormatVersion);

            if (topicState.MessageFormatVersion == topicConfig.MessageFormatVersion)
                return new AlterHandlerResult(false, ConfigEntry, topicState.MessageFormatVersion, topicConfig.MessageFormatVersion);

            return new AlterHandlerResult(true, ConfigEntry, topicState.MessageFormatVersion, topicConfig.MessageFormatVersion);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.MessageFormatVersion = topicState.MessageFormatVersion;
        }
    }
}