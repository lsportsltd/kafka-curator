using System;
using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering.Handlers
{
    public class RetentionBytesAlterHandler : TopicAlterBase
    {
        public override ConfigEntry ConfigEntry => ConfigEntry.RetentionBytes;
        public override AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            if (topicConfig.RetentionBytes == null) return new AlterHandlerResult(ConfigEntry, topicConfig.RetentionBytes);
            
            if (topicState.RetentionBytes == topicConfig.RetentionBytes) return new AlterHandlerResult(false, ConfigEntry, topicState.RetentionBytes, topicConfig.RetentionBytes);

            return new AlterHandlerResult(true, ConfigEntry, topicState.CleanupPolicy, topicConfig.RetentionBytes);
        }

        public override void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState)
        {
            topicConfig.RetentionBytes = topicState.RetentionBytes;
        }
    }
}