using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class SegmentMsAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.SegmentMs;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.SegmentMs == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.SegmentMs);
            
            if (topicState.SegmentMs == topicConfig.SegmentMs) return new AlterHandlerResult(false, ConfigEntry, topicState.SegmentMs, topicConfig.SegmentMs);

            return new AlterHandlerResult(true, ConfigEntry, topicState.SegmentMs, topicConfig.SegmentMs);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.SegmentMs = topicState.SegmentMs;
        }
    }
}