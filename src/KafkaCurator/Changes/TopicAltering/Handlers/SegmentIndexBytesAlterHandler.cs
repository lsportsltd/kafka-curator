using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class SegmentIndexBytesAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.SegmentIndexBytes;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.SegmentIndexBytes == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.SegmentIndexBytes);

            if (topicState.SegmentIndexBytes == topicConfig.SegmentIndexBytes)
                return new AlterHandlerResult(false, ConfigEntry, topicState.SegmentIndexBytes, topicConfig.SegmentIndexBytes);

            return new AlterHandlerResult(true, ConfigEntry, topicState.SegmentIndexBytes, topicConfig.SegmentIndexBytes);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.SegmentIndexBytes = topicState.SegmentIndexBytes;
        }
    }
}