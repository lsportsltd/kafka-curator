using System;
using KafkaCurator.Core.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LSports.Extensions.Kafka.Curator
{
    public static class KafkaClientExtensions
    {
        public static IServiceCollection AddKafkaClient(this IServiceCollection services, Action<KafkaClientOptions> options)
        {
            if(options == null) throw new ArgumentNullException(nameof(options));

            var clientOptions = new KafkaClientOptions();
            options.Invoke(clientOptions);
            
            clientOptions.SanityCheck();

            services.AddSingleton<IKafkaClient>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<IKafkaClient>>();
                var configuration = provider.GetRequiredService<IConfiguration>();
                
                return new KafkaClient(clientOptions, logger, configuration);
            });

            return services;
        }
    }
}