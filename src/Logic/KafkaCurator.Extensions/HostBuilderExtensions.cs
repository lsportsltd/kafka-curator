using Microsoft.Extensions.Hosting;

namespace KafkaCurator.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureEnvironment(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration((context, builder) => context.SetEnvironmentName());
        }

        public static IHostBuilder ConfigureAppSettings(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration((context, builder) =>
            {
                var env = context.HostingEnvironment.EnvironmentName;
                builder.AddAppSettings()
                    .AddTopicSettings(env);
            });
        }

        public static IHostBuilder ConfigureAwsSsm(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddAwsSsm();
            });
        }
    }
}