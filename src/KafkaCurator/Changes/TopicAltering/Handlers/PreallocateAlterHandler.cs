using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class PreallocateAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.Preallocate;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.Preallocate == null || topicConfig.Preallocate == topicState.Preallocate)
                return new AlterHandlerResult();

            return new AlterHandlerResult(true, ConfigEntry, topicConfig.Preallocate);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.Preallocate = topicState.Preallocate;
        }
    }
}