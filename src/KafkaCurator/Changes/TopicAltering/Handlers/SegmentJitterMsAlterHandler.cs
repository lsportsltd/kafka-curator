using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class SegmentJitterMsAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.SegmentJitterMs;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.SegmentJitterMs == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.SegmentJitterMs);

            if (topicState.SegmentJitterMs == topicConfig.SegmentJitterMs)
                return new AlterHandlerResult(false, ConfigEntry, topicState.SegmentJitterMs, topicConfig.SegmentJitterMs);

            return new AlterHandlerResult(true, ConfigEntry, topicState.SegmentJitterMs, topicConfig.SegmentJitterMs);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.SegmentJitterMs = topicState.SegmentJitterMs;
        }
    }
}