using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class CleanupPolicyAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.CleanupPolicy;
        
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.CleanupPolicy == null) return new AlterHandlerResult(ConfigEntry, topicConfig.CleanupPolicy);

            if (topicState.CleanupPolicy == topicConfig.CleanupPolicy) return new AlterHandlerResult(false, ConfigEntry, topicState.CleanupPolicy, topicConfig.CleanupPolicy);

            return new AlterHandlerResult(true, ConfigEntry, topicState.CleanupPolicy, topicConfig.CleanupPolicy);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicState.CleanupPolicy = topicConfig.CleanupPolicy;
        }
    }
}