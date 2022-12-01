using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes
{
    public interface IChangesManager
    {
        Task Handle(IReadOnlyList<ITopicConfiguration> topicConfiguration, IReadOnlyList<ITopicConfiguration> currentState);
    }
}