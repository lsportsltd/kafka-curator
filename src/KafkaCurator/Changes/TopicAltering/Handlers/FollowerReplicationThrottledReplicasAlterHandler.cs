using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class FollowerReplicationThrottledReplicasAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.FollowerReplicationThrottledReplicas;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.FollowerReplicationThrottledReplicas == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.FollowerReplicationThrottledReplicas);

            if (topicState.FollowerReplicationThrottledReplicas == topicConfig.FollowerReplicationThrottledReplicas)
                return new AlterHandlerResult(false, ConfigEntry, topicState.FollowerReplicationThrottledReplicas, topicConfig.FollowerReplicationThrottledReplicas);

            return new AlterHandlerResult(true, ConfigEntry, topicState.FollowerReplicationThrottledReplicas, topicConfig.FollowerReplicationThrottledReplicas);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.FollowerReplicationThrottledReplicas = topicState.FollowerReplicationThrottledReplicas;
        }
    }
}