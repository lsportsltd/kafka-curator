using System;
using KafkaCurator.Abstractions;
using KafkaCurator.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaCurator.Extensions.Microsoft.DependencyInjection
{
    /// <summary>
    /// Extension methods over IServiceProvider
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Creates a KafkaFlow bus
        /// </summary>
        /// <param name="provider">Instance of <see cref="IServiceProvider"/></param>
        /// <returns><see cref="IKafkaCurator"/>A KafkaFlow bus</returns>
        public static IKafkaCurator CreateCurator(this IServiceProvider provider)
        {
            var resolver = provider.GetRequiredService<IDependencyResolver>();
            return provider.GetRequiredService<KafkaCuratorConfigurator>().CreateBus(resolver);
        }
    }
}