using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class MinCleanableDirtyRationAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.MinCleanableDirtyRation;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.MinCleanableDirtyRation == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.MinCleanableDirtyRation);

            if (topicState.MinCleanableDirtyRation == topicConfig.MinCleanableDirtyRation)
                return new AlterHandlerResult(false, ConfigEntry, topicState.MinCleanableDirtyRation, topicConfig.MinCleanableDirtyRation);

            return new AlterHandlerResult(true, ConfigEntry, topicState.MinCleanableDirtyRation, topicConfig.MinCleanableDirtyRation);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.MinCleanableDirtyRation = topicState.MinCleanableDirtyRation;
        }
    }
}