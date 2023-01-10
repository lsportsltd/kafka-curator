using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using KafkaCurator.Abstractions;
using KafkaCurator.Abstractions.Extensions;
using KafkaCurator.Extensions;

namespace KafkaCurator.Configuration
{
    public class ChangesManagerManagerConfigurationBuilder : IChangesManagerConfigurationBuilder
    {
        public IDependencyConfigurator DependencyConfigurator { get; }

        private readonly List<string> _prefixes = new();

        private AdminClientConfig _adminClientConfig;
        private TimeSpan _timeout = TimeSpan.FromSeconds(30);

        private StateManagerConfigurationBuilder _stateManager;

        public ChangesManagerManagerConfigurationBuilder(IDependencyConfigurator dependencyConfigurator)
        {
            DependencyConfigurator = dependencyConfigurator;
        }

        public IChangesManagerConfigurationBuilder WithAdminConfig(AdminClientConfig config)
        {
            _adminClientConfig = config;
            return this;
        }

        public IChangesManagerConfigurationBuilder WithTopicPrefixToExclude(string prefix)
        {
            _prefixes.Add(prefix);
            return this;
        }

        public IChangesManagerConfigurationBuilder WithTopicPrefixToExclude(IEnumerable<string> prefixes)
        {
            _prefixes.AddRange(prefixes);
            return this;
        }

        public IChangesManagerConfigurationBuilder WithTopicPrefixToExclude(params string[] prefixes)
        {
            _prefixes.AddRange(prefixes);
            return this;
        }

        public IChangesManagerConfigurationBuilder WithTimeout(TimeSpan timeout)
        {
            _timeout = timeout;
            return this;
        }

        public IChangesManagerConfigurationBuilder UseStateManager<TStateManager>(
            Action<IStateManagerConfigurationBuilder> stateManager) where TStateManager : class, IStateManager
        {
            DependencyConfigurator.AddSingleton<TStateManager>();

            var builder = new StateManagerConfigurationBuilder(DependencyConfigurator, typeof(TStateManager));

            stateManager(builder);

            _stateManager = builder;

            return this;
        }

        public ChangesManagerConfiguration Build(ClusterConfiguration clusterConfiguration)
        {
            _adminClientConfig ??= new AdminClientConfig();

            _adminClientConfig.BootstrapServers = clusterConfiguration.Brokers;
            _adminClientConfig.ReadSecurityInformation(clusterConfiguration);

            _stateManager ??= new StateManagerConfigurationBuilder(DependencyConfigurator);
            
            return new ChangesManagerConfiguration(clusterConfiguration.Name, _adminClientConfig,
                _prefixes.AsReadOnly(), _timeout, _stateManager.Build());
        }
    }
}