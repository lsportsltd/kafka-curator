using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class MaxMessageBytesAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.MaxMessageBytes;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.MaxMessageBytes == null) return new AlterHandlerResult(ConfigEntry, topicConfig.MaxMessageBytes);

            if (topicState.MaxMessageBytes == topicConfig.MaxMessageBytes)
                return new AlterHandlerResult(false, ConfigEntry, topicState.MaxMessageBytes, topicConfig.MaxMessageBytes);

            return new AlterHandlerResult(true, ConfigEntry, topicState.MaxMessageBytes, topicConfig.MaxMessageBytes);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.MaxMessageBytes = topicState.MaxMessageBytes;
        }
    }
}