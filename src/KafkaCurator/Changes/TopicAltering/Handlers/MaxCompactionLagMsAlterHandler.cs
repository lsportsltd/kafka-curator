using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class MaxCompactionLagMsAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.MaxCompactionLagMs;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.MaxCompactionLagMs == null || topicConfig.MaxCompactionLagMs == topicState.MaxCompactionLagMs)
                return new AlterHandlerResult();

            return new AlterHandlerResult(true, ConfigEntry, topicConfig.MaxCompactionLagMs);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.MaxCompactionLagMs = topicState.MaxCompactionLagMs;
        }
    }
}