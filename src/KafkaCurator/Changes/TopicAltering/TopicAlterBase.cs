using KafkaCurator.Abstractions.Configuration;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes.TopicAltering
{
    public abstract class TopicAlterBase : ITopicAlterHandler
    {
        public abstract ConfigEntry ConfigEntry { get; }
        public abstract AlterHandlerResult ShouldAlter(ITopicConfiguration topicConfig, ITopicConfiguration topicState);
        public abstract void Alter(ITopicConfiguration topicConfig, ITopicConfiguration topicState);
    }

    public class AlterHandlerResult
    {
        public bool ShouldAlter { get; }
        public ConfigEntry ConfigEntry { get; }
        public string NewValue { get; }
        public string OldValue { get; }

        public AlterHandlerResult()
        {
            ShouldAlter = false;
        }

        public AlterHandlerResult(ConfigEntry configEntry, string oldValue)
        {
            ShouldAlter = false;
            ConfigEntry = configEntry;
            OldValue = oldValue;
        }

        public AlterHandlerResult(bool result, ConfigEntry entry, string oldValue, string newValue)
        {
            ShouldAlter = result;
            ConfigEntry = entry;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}