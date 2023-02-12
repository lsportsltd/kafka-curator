using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class FlushMsAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.FlushMs;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.FlushMs == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.FlushMs);
            
            if (topicState.FlushMs == topicConfig.FlushMs) return new AlterHandlerResult(false, ConfigEntry, topicState.FlushMs, topicConfig.FlushMs);

            return new AlterHandlerResult(true, ConfigEntry, topicState.FlushMs, topicConfig.FlushMs);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.FlushMs = topicState.FlushMs;
        }
    }
}