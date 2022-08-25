using System;
using KafkaCurator.Core.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KafkaCurator.Extensions
{
    public static class KafkaClientExtensions
    {
        public static IServiceCollection AddKafkaClient(this IServiceCollection services, Action<KafkaClientOptions> options)
        {
            if(options == null) throw new ArgumentNullException(nameof(options));

            var clientOptions = new KafkaClientOptions();
            options.Invoke(clientOptions);

            services.AddSingleton<IKafkaClient>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<IKafkaClient>>();

                return new KafkaClient(clientOptions.BootstrapServers, logger);
            });

            return services;
        }
    }

    public class KafkaClientOptions
    {
        public string BootstrapServers { get; set; }
    }
}