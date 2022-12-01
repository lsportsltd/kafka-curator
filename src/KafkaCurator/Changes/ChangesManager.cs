using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaCurator.Abstractions;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes
{
    internal class ChangesManager : IChangesManager
    {
        private readonly ILogHandler _logHandler;
        private readonly IChangesManagerConfiguration _changesManagerConfiguration;
        private readonly IAdminClient _adminClient;

        public ChangesManager(ILogHandler logHandler, IChangesManagerConfiguration changesManagerConfiguration, IAdminClientFactory adminClientFactory)
        {
            _logHandler = logHandler;
            _changesManagerConfiguration = changesManagerConfiguration;
            _adminClient = adminClientFactory.GetOrCreate(changesManagerConfiguration.ClusterName, changesManagerConfiguration.AdminClientConfig);
        }

        public Task Handle(IReadOnlyList<ITopicConfiguration> topicConfiguration, IReadOnlyList<ITopicConfiguration> currentState)
        {
            throw new System.NotImplementedException();
        }
    }
}