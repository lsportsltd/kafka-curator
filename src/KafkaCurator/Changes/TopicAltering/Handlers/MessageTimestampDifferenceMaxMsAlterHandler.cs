using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class MessageTimestampDifferenceMaxMsAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.MessageTimestampDifferenceMaxMs;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.MessageTimestampDifferenceMaxMs == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.MessageTimestampDifferenceMaxMs);

            if (topicState.MessageTimestampDifferenceMaxMs == topicConfig.MessageTimestampDifferenceMaxMs)
                return new AlterHandlerResult(false, ConfigEntry, topicState.MessageTimestampDifferenceMaxMs, topicConfig.MessageTimestampDifferenceMaxMs);

            return new AlterHandlerResult(true, ConfigEntry, topicState.MessageTimestampDifferenceMaxMs, topicConfig.MessageTimestampDifferenceMaxMs);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.MessageTimestampDifferenceMaxMs = topicState.MessageTimestampDifferenceMaxMs;
        }
    }
}