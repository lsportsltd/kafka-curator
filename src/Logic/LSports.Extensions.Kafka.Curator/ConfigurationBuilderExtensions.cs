using Microsoft.Extensions.Configuration;

namespace LSports.Extensions.Kafka.Curator
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder configBuilder)
        {
            return configBuilder.AddJsonFile($"appsettings.json", optional: true);
        }

        public static IConfigurationBuilder AddTopicSettings(this IConfigurationBuilder configBuilder, string environment)
        {
            return configBuilder.AddJsonFile($"topicsettings.{environment}.json", optional: false);
        }
    }
}
