using System;
using System.Collections.Generic;
using KafkaCurator.Abstractions;
using KafkaCurator.Abstractions.Configuration;

namespace KafkaCurator.Configuration
{
    public class ClusterConfiguration
    {
        public KafkaConfiguration Kafka { get; }
        public string Name { get; }
        public string Brokers { get; }
        public IReadOnlyList<ITopicConfiguration> Topics => _topics.AsReadOnly();
        public IChangesManagerConfiguration ChangesManager { get; private set; }

        private readonly Func<SecurityInformation> _securityInformationHandler;
        private readonly List<ITopicConfiguration> _topics = new();
        private Func<IDependencyResolver, IStateConfigurator> _stateConfigurator;

        public ClusterConfiguration(KafkaConfiguration kafka, string name, string brokers,
            Func<SecurityInformation> securityInformationHandler)
        {
            Kafka = kafka;
            Name = name ?? Guid.NewGuid().ToString();
            Brokers = brokers;
            
            _securityInformationHandler = securityInformationHandler;
        }

        public void AddTopics(IEnumerable<ITopicConfiguration> configurations) => _topics.AddRange(configurations);
        public SecurityInformation GetSecurityInformation() => _securityInformationHandler?.Invoke();
        public IStateConfigurator GetStateConfigurator(IDependencyResolver resolver) => _stateConfigurator?.Invoke(resolver);
        public void AddChangesManager(IChangesManagerConfiguration changesManagerConfiguration) => ChangesManager = changesManagerConfiguration;
        public void AddStateConfigurator(Func<IDependencyResolver, IStateConfigurator> stateConfigurator) => _stateConfigurator = stateConfigurator;
    }
}