using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaCurator.Configuration;

namespace KafkaCurator
{
    /// <summary>
    /// An interface used to create a state handler
    /// </summary>
    public interface IStateHandler
    {
        List<ITopicConfiguration> GetState();
        Task<List<ITopicConfiguration>> GetStateAsync();
        
        void SetState(IEnumerable<ITopicConfiguration> state);
        Task SetStateAsync(IEnumerable<ITopicConfiguration> state);
    }
}