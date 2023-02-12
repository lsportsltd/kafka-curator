using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class PreallocateAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.Preallocate;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.Preallocate == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.Preallocate);
            
            if (topicState.Preallocate == topicConfig.Preallocate) return new AlterHandlerResult(false, ConfigEntry, topicState.Preallocate, topicConfig.Preallocate);

            return new AlterHandlerResult(true, ConfigEntry, topicState.Preallocate, topicConfig.Preallocate);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.Preallocate = topicState.Preallocate;
        }
    }
}