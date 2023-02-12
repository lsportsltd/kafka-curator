using Confluent.Kafka;
using KafkaCurator.Abstractions;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes
{
    public abstract class TopicsHandlerBase
    {
        protected readonly IDependencyResolver DependencyResolver;
        protected readonly IAdminClient AdminClient;
        
        private readonly ChangesManagerConfiguration _configuration;

        public TopicsHandlerBase(IDependencyResolver dependencyResolver, ChangesManagerConfiguration configuration)
        {
            DependencyResolver = dependencyResolver;
            _configuration = configuration;

            AdminClient = GetAdminClient();
        }

        protected IAdminClient GetAdminClient()
        {
            var adminClientFactory = DependencyResolver.Resolve<IAdminClientFactory>();

            return adminClientFactory.GetOrCreate(_configuration.ClusterName,
                _configuration.AdminClientConfig);
        }
    }
}