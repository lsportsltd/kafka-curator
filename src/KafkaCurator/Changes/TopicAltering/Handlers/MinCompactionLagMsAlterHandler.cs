using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class MinCompactionLagMsAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.MinCompactionLagMs;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.MinCompactionLagMs == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.MinCompactionLagMs);

            if (topicState.MinCompactionLagMs == topicConfig.MinCompactionLagMs)
                return new AlterHandlerResult(false, ConfigEntry, topicState.MinCompactionLagMs, topicConfig.MinCompactionLagMs);

            return new AlterHandlerResult(true, ConfigEntry, topicState.MinCompactionLagMs, topicConfig.MinCompactionLagMs);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.MinCompactionLagMs = topicState.MinCompactionLagMs;
        }
    }
}