using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class UncleanLeaderElectionEnableAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.UncleanLeaderElectionEnable;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.UncleanLeaderElectionEnable == null || topicConfig.UncleanLeaderElectionEnable == topicState.UncleanLeaderElectionEnable)
                return new AlterHandlerResult();

            return new AlterHandlerResult(true, ConfigEntry, topicConfig.UncleanLeaderElectionEnable);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.UncleanLeaderElectionEnable = topicState.UncleanLeaderElectionEnable;
        }
    }
}