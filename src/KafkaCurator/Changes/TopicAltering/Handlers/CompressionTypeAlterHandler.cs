using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class CompressionTypeAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.CompressionType;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.CompressionType == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.CompressionType);
            
            if (topicState.CompressionType == topicConfig.CompressionType) return new AlterHandlerResult(false, ConfigEntry, topicState.CompressionType, topicConfig.CompressionType);

            return new AlterHandlerResult(true, ConfigEntry, topicState.CompressionType, topicConfig.CompressionType);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.CompressionType = topicState.CompressionType;
        }
    }
}