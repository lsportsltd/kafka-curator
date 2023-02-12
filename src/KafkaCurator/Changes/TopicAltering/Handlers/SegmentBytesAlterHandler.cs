using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class SegmentBytesAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.SegmentBytes;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.SegmentBytes == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.SegmentBytes);

            if (topicState.SegmentBytes == topicConfig.SegmentBytes) return new AlterHandlerResult(false, ConfigEntry, topicState.SegmentBytes, topicConfig.SegmentBytes);

            return new AlterHandlerResult(true, ConfigEntry, topicState.SegmentBytes, topicConfig.SegmentBytes);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.SegmentBytes = topicState.SegmentBytes;
        }
    }
}