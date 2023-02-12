using System;
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

        public async Task<int> PreviewAsync(CancellationToken cancellationToken = default)
        {
            var changesManagerAccessor = _dependencyResolver.Resolve<IChangesManagerAccessor>();
            
            foreach (var cluster in _kafkaConfiguration.Clusters)
            {
                var changesManager = changesManagerAccessor.GetChangesManager(cluster.ChangesManager.ClusterName);
                await changesManager.PreviewChanges(cluster.Topics);
            }

            return 0;
        }

        public async Task<int> RunAsync(RunConfiguration runConfiguration, CancellationToken cancellationToken = default)
        {
            var logger = _dependencyResolver.Resolve<ILogHandler>();

            try
            {
                await PreviewAsync(cancellationToken);
            }
            catch (Exception e)
            {
                logger.Error("An error has occurred while previewing changes.", e, null);
                return 1;
            }

            if (runConfiguration.DryRun) return 1;
            
            try
            {
                return await ExecuteAsync(cancellationToken);
            }
            catch (Exception e)
            {
                logger.Error("An error has occurred while executing KafkaCurator.", e, null);
                return 1;
            }
        }
    }
}