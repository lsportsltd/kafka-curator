using KafkaCurator.Abstractions;

namespace KafkaCurator
{
    public interface IStateConfigurator
    {
        IStateHandler CreateStateHandler(IDependencyResolver resolver);
    }
}