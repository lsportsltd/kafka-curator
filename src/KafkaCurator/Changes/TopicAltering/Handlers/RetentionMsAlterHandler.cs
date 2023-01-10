using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class RetentionMsAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.RetentionMs;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.RetentionMs == null || topicConfig.RetentionMs == topicState.RetentionMs)
                return new AlterHandlerResult();

            return new AlterHandlerResult(true, ConfigEntry, topicConfig.RetentionMs);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.RetentionMs = topicState.RetentionMs;
        }
    }
}