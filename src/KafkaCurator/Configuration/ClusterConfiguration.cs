using System;
using System.Collections.Generic;
using KafkaCurator.Abstractions.Configuration;

namespace KafkaCurator.Configuration
{
    public class ClusterConfiguration
    {
        public KafkaConfiguration Kafka { get; }
        public string Name { get; }
        public string Brokers { get; }
        public IReadOnlyList<ITopicConfiguration> Topics => _topics.AsReadOnly();
        public ChangesManagerConfiguration ChangesManager { get; private set; }

        private readonly Func<SecurityInformation> _securityInformationHandler;
        private readonly List<ITopicConfiguration> _topics = new();

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
        public void AddChangesManager(ChangesManagerConfiguration changesManagerConfiguration) => ChangesManager = changesManagerConfiguration;
    }
}