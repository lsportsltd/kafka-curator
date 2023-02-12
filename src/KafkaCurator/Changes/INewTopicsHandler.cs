using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes
{
    public interface INewTopicsHandler
    {
        internal string Name { get; }
        int PreviewNewTopics(IReadOnlyCollection<ITopicConfiguration> newTopics);
        Task<int> HandleNewTopics(IReadOnlyCollection<ITopicConfiguration> newTopics);
    }
}