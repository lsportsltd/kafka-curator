using System.Threading.Tasks;
using KafkaCurator.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KafkaCurator
{
    class Program
    {
        static Task Main(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureEnvironment()
                .ConfigureAppSettings()
                .ConfigureAwsSsm()
                .ConfigureServices(collection =>
                {
                    collection.AddHostedService<KafkaCuratorHostedService>();
                })
                .Build()
                .RunAsync();
        }
    }
}
