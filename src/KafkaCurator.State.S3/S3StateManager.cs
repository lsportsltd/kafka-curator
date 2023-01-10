using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaCurator.Configuration;

namespace KafkaCurator.State.S3
{
    public class S3StateManager : IStateManager
    {
        public Task<IReadOnlyList<ITopicConfiguration>> GetState(IStateParameters stateParameters)
        {
            throw new System.NotImplementedException();
        }
    }
}