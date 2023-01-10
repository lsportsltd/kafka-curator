using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaCurator.Configuration;

namespace KafkaCurator
{
    public interface IStateManager
    {
        Task<IReadOnlyList<ITopicConfiguration>> GetState(IStateParameters stateParameters);
        Task SetState(IReadOnlyList<ITopicConfiguration> topicConfigurations);
    }
}