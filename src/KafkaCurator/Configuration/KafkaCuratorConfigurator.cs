using System;
using KafkaCurator.Abstractions;

namespace KafkaCurator.Configuration
{
    public class KafkaCuratorConfigurator
    {
        private readonly KafkaConfiguration _configuration;

        public KafkaCuratorConfigurator(IDependencyConfigurator dependencyConfigurator, Action<IKafkaConfigurationBuilder> kafka)
        {
            var builder = new KafkaConfigurationBuilder(dependencyConfigurator);

            kafka(builder);

            _configuration = builder.Build();
        }

        public IKafkaCurator CreateBus(IDependencyResolver resolver)
        {
            var scope = resolver.CreateScope();

            var changesManagerFactory = scope.Resolver.Resolve<IChangesManagerFactory>();
            var adminClientFactory = scope.Resolver.Resolve<IAdminClientFactory>();
            
            return new KafkaCurator(_configuration, scope.Resolver, changesManagerFactory, adminClientFactory);
        }
    }
}