using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class RetentionMsAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.RetentionMs;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.RetentionMs == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.RetentionMs);
            
            if (topicState.RetentionMs == topicConfig.RetentionMs) return new AlterHandlerResult(false, ConfigEntry, topicState.RetentionMs, topicConfig.RetentionMs);

            return new AlterHandlerResult(true, ConfigEntry, topicState.RetentionMs, topicConfig.RetentionMs);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.RetentionMs = topicState.RetentionMs;
        }
    }
}