using KafkaCurator.Abstractions;
using KafkaCurator.Changes;
using KafkaCurator.Configuration;

namespace KafkaCurator
{
    public interface IChangesManagerFactory
    {
        IChangesManager Create(IChangesManagerConfiguration configuration, IDependencyResolver resolver);
    }
}