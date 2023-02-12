using System.Threading.Tasks;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes
{
    public interface IExistingTopicsHandler
    {
        internal string Name { get; }
        int PreviewExistingTopics((ITopicConfiguration, ITopicConfiguration)[] topicPairs);
        Task<int> HandleExistingTopics((ITopicConfiguration, ITopicConfiguration)[] topicPairs);
    }
}