using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class FlushMessagesAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.FlushMessages;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.FlushMessages == null)
                return new AlterHandlerResult(ConfigEntry, topicConfig.FlushMessages);
            
            if (topicState.FlushMessages == topicConfig.FlushMessages) return new AlterHandlerResult(false, ConfigEntry, topicState.FlushMessages, topicConfig.FlushMessages);

            return new AlterHandlerResult(true, ConfigEntry, topicState.FlushMessages, topicConfig.FlushMessages);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.FlushMessages = topicState.FlushMessages;
        }
    }
}