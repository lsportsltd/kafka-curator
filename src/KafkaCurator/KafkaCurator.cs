using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KafkaCurator.Abstractions;
using KafkaCurator.Changes;
using KafkaCurator.Configuration;

namespace KafkaCurator
{
    public class KafkaCurator : IKafkaCurator
    {
        private readonly KafkaConfiguration _kafkaConfiguration;
        private readonly IDependencyResolver _dependencyResolver;
        private readonly IChangesManagerFactory _changesManagerFactory;
        private readonly IAdminClientFactory _adminClientFactory;

        public KafkaCurator(KafkaConfiguration kafkaConfiguration, IDependencyResolver dependencyResolver, IChangesManagerFactory changesManagerFactory, IAdminClientFactory adminClientFactory)
        {
            _kafkaConfiguration = kafkaConfiguration;
            _dependencyResolver = dependencyResolver;
            _changesManagerFactory = changesManagerFactory;
            _adminClientFactory = adminClientFactory;
        }
        
        public async Task<int> ExecuteAsync(CancellationToken stopCancellationToken = default)
        {
            var changesManagerAccessor = _dependencyResolver.Resolve<IChangesManagerAccessor>();
            
            foreach (var cluster in _kafkaConfiguration.Clusters)
            {
                var changesManager = changesManagerAccessor.GetChangesManager(cluster.ChangesManager.ClusterName);
                await changesManager.HandleChanges(cluster.Topics);
            }

            return 0;
        }

        public Task<int> PreviewAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}