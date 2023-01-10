using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class MessageTimestampDifferenceMaxMsAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.MessageTimestampDifferenceMaxMs;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.MessageTimestampDifferenceMaxMs == null || topicConfig.MessageTimestampDifferenceMaxMs == topicState.MessageTimestampDifferenceMaxMs)
                return new AlterHandlerResult();

            return new AlterHandlerResult(true, ConfigEntry, topicConfig.MessageTimestampDifferenceMaxMs);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.MessageTimestampDifferenceMaxMs = topicState.MessageTimestampDifferenceMaxMs;
        }
    }
}