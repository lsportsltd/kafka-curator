using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class UncleanLeaderElectionEnableAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.UncleanLeaderElectionEnable;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.UncleanLeaderElectionEnable == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.UncleanLeaderElectionEnable);

            if (topicState.UncleanLeaderElectionEnable == topicConfig.UncleanLeaderElectionEnable)
                return new AlterHandlerResult(false, ConfigEntry, topicState.UncleanLeaderElectionEnable, topicConfig.UncleanLeaderElectionEnable);

            return new AlterHandlerResult(true, ConfigEntry, topicState.UncleanLeaderElectionEnable, topicConfig.UncleanLeaderElectionEnable);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.UncleanLeaderElectionEnable = topicState.UncleanLeaderElectionEnable;
        }
    }
}