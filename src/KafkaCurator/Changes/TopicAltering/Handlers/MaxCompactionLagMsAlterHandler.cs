using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class MaxCompactionLagMsAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.MaxCompactionLagMs;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.MaxCompactionLagMs == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.MaxCompactionLagMs);

            if (topicState.MaxCompactionLagMs == topicConfig.MaxCompactionLagMs)
                return new AlterHandlerResult(false, ConfigEntry, topicState.MaxCompactionLagMs, topicConfig.MaxCompactionLagMs);

            return new AlterHandlerResult(true, ConfigEntry, topicState.MaxCompactionLagMs, topicConfig.MaxCompactionLagMs);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.MaxCompactionLagMs = topicState.MaxCompactionLagMs;
        }
    }
}