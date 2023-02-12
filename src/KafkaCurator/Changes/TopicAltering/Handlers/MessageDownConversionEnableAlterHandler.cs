using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class MessageDownConversionEnableAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.MessageDownConversionEnable;

        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.MessageDownConversionEnable == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.MessageDownConversionEnable);

            if (topicState.MessageDownConversionEnable == topicConfig.MessageDownConversionEnable)
                return new AlterHandlerResult(false, ConfigEntry, topicState.MessageDownConversionEnable, topicConfig.MessageDownConversionEnable);

            return new AlterHandlerResult(true, ConfigEntry, topicState.MessageDownConversionEnable, topicConfig.MessageDownConversionEnable);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.MessageDownConversionEnable = topicState.MessageDownConversionEnable;
        }
    }
}