using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class FollowerReplicationThrottledReplicasAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.FollowerReplicationThrottledReplicas;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.FollowerReplicationThrottledReplicas == null || topicConfig.FollowerReplicationThrottledReplicas == topicState.FollowerReplicationThrottledReplicas)
                return new AlterHandlerResult();

            return new AlterHandlerResult(true, ConfigEntry, topicConfig.FollowerReplicationThrottledReplicas);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.FollowerReplicationThrottledReplicas = topicState.FollowerReplicationThrottledReplicas;
        }
    }
}