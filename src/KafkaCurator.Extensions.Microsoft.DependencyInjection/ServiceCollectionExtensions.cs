using System;
using KafkaCurator.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KafkaCurator.Microsoft.DependencyInjection;

namespace KafkaCurator.Extensions.Microsoft.DependencyInjection
{
    /// <summary>
    /// Extension methods over IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures KafkaCurator
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/></param>
        /// <param name="kafka">A handler to configure KafkaCurator</param>
        /// <returns></returns>
        public static IServiceCollection AddKafkaCurator(
            this IServiceCollection services,
            Action<IKafkaConfigurationBuilder> kafka)
        {
            var configurator = new KafkaCuratorConfigurator(
                new MicrosoftDependencyConfigurator(services),
                kafka);

            return services.AddSingleton(configurator);
        }
    }
}