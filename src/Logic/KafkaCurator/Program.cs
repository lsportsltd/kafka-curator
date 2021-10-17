using System.Threading.Tasks;
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
                .ConfigureServices(collection =>
                {
                    collection.AddHostedService<KafkaCuratorHostedService>();
                })
                .Build()
                .RunAsync();
        }
    }
}
