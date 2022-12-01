using System;
using System.Threading;
using System.Threading.Tasks;
using KafkaCurator.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KafkaCurator.Extensions.Hosting
{
    public class KafkaCuratorBackgroundService : BackgroundService
    {
        private readonly IKafkaCurator _curator;

        public KafkaCuratorBackgroundService(IServiceProvider serviceProvider)
        {
            _curator = serviceProvider.CreateCurator();
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _curator.ExecuteAsync(stoppingToken);
        }
    }
}