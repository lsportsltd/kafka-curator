using System;
using KafkaCurator.Configuration;
using KafkaCurator.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaCurator.Extensions.Hosting
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaCuratorBackgroundService(this IServiceCollection services,
            Action<IKafkaConfigurationBuilder> kafka)
        {
            return services.AddHostedService<KafkaCuratorBackgroundExecutionService>()
                .AddKafkaCurator(kafka);
        }
    }
}