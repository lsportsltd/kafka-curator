using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KafkaCurator.Abstractions;
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
        
        public async Task ExecuteAsync(CancellationToken stopCancellationToken = default)
        {
            foreach (var cluster in _kafkaConfiguration.Clusters)
            {
                var changesManager = _changesManagerFactory.Create(cluster.ChangesManager, _dependencyResolver);
                var stateHandler = GetStateConfigurator(cluster).CreateStateHandler(_dependencyResolver);

                var topicsState = await stateHandler.GetStateAsync();

                await changesManager.Handle(cluster.Topics, topicsState.AsReadOnly());
            }
        }

        private IStateConfigurator GetStateConfigurator(ClusterConfiguration clusterConfiguration)
        {
            var stateConfigurator = clusterConfiguration.GetStateConfigurator(_dependencyResolver);
            if (stateConfigurator != null) return stateConfigurator;

            return new DefaultStateConfigurator(_adminClientFactory.GetOrCreate(clusterConfiguration.Name,
                clusterConfiguration.ChangesManager.AdminClientConfig), clusterConfiguration.ChangesManager.Timeout);
        }
    }
}