using KafkaCurator.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaCurator.Microsoft.DependencyInjection
{
    public class MicrosoftDependencyResolverScope : IDependencyResolverScope
    {
        public IDependencyResolver Resolver { get; }

        private readonly IServiceScope _scope;

        public MicrosoftDependencyResolverScope(IServiceScope scope)
        {
            _scope = scope;
            Resolver = new MicrosoftDependencyResolver(scope.ServiceProvider);
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}