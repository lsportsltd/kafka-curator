using KafkaCurator.Abstractions;
using KafkaCurator.Changes;
using KafkaCurator.Configuration;

namespace KafkaCurator
{
    public class ChangesManagerFactory : IChangesManagerFactory
    {
        public IChangesManager Create(IChangesManagerConfiguration configuration, IDependencyResolver resolver)
        {
            var logHandler = resolver.Resolve<ILogHandler>();
            var adminClientFactory = resolver.Resolve<IAdminClientFactory>();

            return new ChangesManager(logHandler, configuration, adminClientFactory);
        }
    }
}