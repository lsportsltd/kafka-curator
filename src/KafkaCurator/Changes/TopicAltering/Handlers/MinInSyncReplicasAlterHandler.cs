using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class MinInSyncReplicasAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.MinInSyncReplicas;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.MinInSyncReplicas == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.MinInSyncReplicas);

            if (topicState.MinInSyncReplicas == topicConfig.MinInSyncReplicas)
                return new AlterHandlerResult(false, ConfigEntry, topicState.MinInSyncReplicas, topicConfig.MinInSyncReplicas);

            return new AlterHandlerResult(true, ConfigEntry, topicState.MinInSyncReplicas, topicConfig.MinInSyncReplicas);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.MinInSyncReplicas = topicState.MinInSyncReplicas;
        }
    }
}