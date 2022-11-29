using Microsoft.Extensions.DependencyInjection;

namespace KafkaCurator.Configuration
{
    public class AdminConfigurationBuilder : IAdminConfigurationBuilder
    {
        public IServiceCollection ServiceCollection { get; }
        
        public AdminConfigurationBuilder(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }
    }
}