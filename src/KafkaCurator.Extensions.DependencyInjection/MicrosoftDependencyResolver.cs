using System;
using System.Collections.Generic;
using KafkaCurator.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaCurator.Extensions.DependencyInjection
{
    public class MicrosoftDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public MicrosoftDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public IDependencyResolverScope CreateScope()
        {
            return new MicrosoftDependencyResolverScope(_serviceProvider.CreateScope());
        }

        public object Resolve(Type type)
        {
            return _serviceProvider.GetService(type);
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            return _serviceProvider.GetServices(type);
        }

        public T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return _serviceProvider.GetServices<T>();
        }
    }
}