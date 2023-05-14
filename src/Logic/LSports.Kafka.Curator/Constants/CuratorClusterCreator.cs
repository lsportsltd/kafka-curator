using KafkaCurator.LogHandler.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KafkaCurator.Extensions.Microsoft.DependencyInjection;
using SecurityProtocol = KafkaCurator.Abstractions.Configuration.SecurityProtocol;

namespace LSports.Kafka.Curator.Constants;

public static class CuratorClusterCreator
{
    public static void CreateBasedOnEvn(string? env,IServiceCollection services, IConfigurationRoot config)
    {
        if (env is "dev")
        {
            services.AddKafkaCurator(kafka => kafka
                .UseConsoleLog()

                .AddCluster(cluster => cluster
                    .WithBrokers(config[Endpoints.KafkaHermesBootstrapServers])
                    .WithSecurityInformation(info => info.SecurityProtocol = SecurityProtocol.Ssl)
                    .ConfigureChangesManager(changes => changes
                        .WithTopicPrefixToExclude(config.GetSection(TopicPattern.ToExcludeHermes).Get<string[]>()))
                    .AddTopicsJsonFile($"topicsettings.hermes.{env}.json"))

                .AddCluster(cluster => cluster
                    .WithBrokers(config[Endpoints.KafkaCobWebBootstrapServers])
                    .WithSecurityInformation(info => info.SecurityProtocol = SecurityProtocol.Ssl)
                    .ConfigureChangesManager(changes => changes
                        .WithTopicPrefixToExclude(config.GetSection(TopicPattern.ToExcludeCobWeb).Get<string[]>()))
                    .AddTopicsJsonFile($"topicsettings.cobweb.{env}.json")));
        }
        else
        {
            services.AddKafkaCurator(kafka => kafka
                .UseConsoleLog()

                .AddCluster(cluster => cluster
                    .WithBrokers(config[Endpoints.KafkaHermesBootstrapServers])
                    .WithSecurityInformation(info => info.SecurityProtocol = SecurityProtocol.Ssl)
                    .ConfigureChangesManager(changes => changes
                        .WithTopicPrefixToExclude(config.GetSection(TopicPattern.ToExcludeHermes).Get<string[]>()))
                    .AddTopicsJsonFile($"topicsettings.hermes.{env}.json")));
        }
    }
}