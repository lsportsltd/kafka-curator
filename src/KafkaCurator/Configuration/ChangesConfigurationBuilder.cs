using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using KafkaCurator.Abstractions;
using KafkaCurator.Extensions;

namespace KafkaCurator.Configuration
{
    public class ChangesConfigurationBuilder : IChangesConfigurationBuilder
    {
        public IDependencyConfigurator DependencyConfigurator { get; }

        private readonly List<string> _prefixes = new();

        private AdminClientConfig _adminClientConfig;
        private TimeSpan _timeout = TimeSpan.FromSeconds(30);
        
        public ChangesConfigurationBuilder(IDependencyConfigurator dependencyConfigurator)
        {
            DependencyConfigurator = dependencyConfigurator;
        }

        public IChangesConfigurationBuilder WithAdminConfig(AdminClientConfig config)
        {
            _adminClientConfig = config;
            return this;
        }

        public IChangesConfigurationBuilder WithTopicPrefixToExclude(string prefix)
        {
            _prefixes.Add(prefix);
            return this;
        }

        public IChangesConfigurationBuilder WithTopicPrefixToExclude(IEnumerable<string> prefixes)
        {
            _prefixes.AddRange(prefixes);
            return this;
        }

        public IChangesConfigurationBuilder WithTimeout(TimeSpan timeout)
        {
            _timeout = timeout;
            return this;
        }

        public ChangesManagerConfiguration Build(ClusterConfiguration clusterConfiguration)
        {
            _adminClientConfig ??= new AdminClientConfig();
            
            _adminClientConfig.BootstrapServers = clusterConfiguration.Brokers;
            _adminClientConfig.ReadSecurityInformation(clusterConfiguration);
            
            return new ChangesManagerConfiguration(clusterConfiguration.Name, _adminClientConfig, _prefixes.AsReadOnly(), _timeout);
        }
    }
}