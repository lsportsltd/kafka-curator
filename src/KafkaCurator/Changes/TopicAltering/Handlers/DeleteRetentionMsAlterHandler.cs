using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class DeleteRetentionMsAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.DeleteRetentionMs;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.DeleteRetentionMs == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.DeleteRetentionMs);

            if (topicState.DeleteRetentionMs == topicConfig.DeleteRetentionMs)
                return new AlterHandlerResult(false, ConfigEntry, topicState.DeleteRetentionMs, topicConfig.DeleteRetentionMs);

            return new AlterHandlerResult(true, ConfigEntry, topicState.DeleteRetentionMs, topicConfig.DeleteRetentionMs);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.DeleteRetentionMs = topicState.DeleteRetentionMs;
        }
    }
}