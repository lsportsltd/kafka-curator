using KafkaCurator.Abstractions;
using KafkaCurator.Changes;
using KafkaCurator.Configuration;

namespace KafkaCurator
{
    public class ChangesManagerFactory : IChangesManagerFactory
    {
        public IChangesManager Create(ChangesManagerConfiguration configuration, IDependencyResolver resolver)
        {
            return new ChangesManager(resolver, configuration);
        }
    }
}