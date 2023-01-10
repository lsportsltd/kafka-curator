using System;
using KafkaCurator.Abstractions;

namespace KafkaCurator.Configuration
{
    public class StateManagerConfigurationBuilder : IStateManagerConfigurationBuilder
    {
        public IDependencyConfigurator DependencyConfigurator { get; }

        private readonly Type _stateManagerType;

        private string _name;
        private TimeSpan _timeout = TimeSpan.FromSeconds(30);

        public StateManagerConfigurationBuilder(IDependencyConfigurator dependencyConfigurator, Type stateManagerType = null)
        {
            DependencyConfigurator = dependencyConfigurator;
            _stateManagerType = stateManagerType;
        }

        public IStateManagerConfigurationBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public IStateManagerConfigurationBuilder WithTimeout(TimeSpan timeout)
        {
            _timeout = timeout;
            return this;
        }

        public StateManagerConfiguration Build()
        {
            return new StateManagerConfiguration(_name, _timeout, _stateManagerType);
        }
    }
}