using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaCurator.Configuration;

namespace KafkaCurator.State
{
    public abstract class StateManagerBase<TStateParameters> : IStateManager where TStateParameters : IStateParameters
    {
        public Task<IReadOnlyList<ITopicConfiguration>> GetState(IStateParameters stateParameters)
        {
            return GetStateInternal((TStateParameters) stateParameters);
        }

        public abstract Task SetState(IReadOnlyList<ITopicConfiguration> topicConfigurations);
        protected abstract Task<IReadOnlyList<ITopicConfiguration>> GetStateInternal(TStateParameters stateParameters);
    }
}