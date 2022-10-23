using System.Threading.Tasks;
using KafkaCurator.Core.Constants;
using KafkaCurator.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KafkaCurator
{
    class Program
    {
        public static Task Main(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureEnvironment()
                .ConfigureAppSettings()
                .ConfigureAwsSsm()
                .ConfigureServices((context, serviceCollection) =>
                {
                    serviceCollection.AddKafkaClient(options =>
                    {
                        options.BootstrapServers = context.Configuration[Endpoints.KafkaBootstrapServers];
                    });
                    serviceCollection.AddHostedService<KafkaCuratorHostedService>();
                })
                .Build()
                .RunAsync();
        }
    }
}
