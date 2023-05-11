using KafkaCurator.Configuration;
using KafkaCurator.LogHandler.Console;
using LSports.Kafka.Curator.Constants;
using LSports.Kafka.Curator.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KafkaCurator.Extensions.Microsoft.DependencyInjection;
using SecurityProtocol = KafkaCurator.Abstractions.Configuration.SecurityProtocol;

var env = Environment.GetEnvironmentVariable(EnvironmentVariableName.HostEnvironment);
var services = new ServiceCollection();

var config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.hermes.json", false)
    .AddJsonFile("appsettings.cobweb.json", false)
    .AddAwsSsm()
    .Build();

#region Hermes Cluster

services.AddKafkaCurator(kafka => kafka
    .UseConsoleLog()
    .AddCluster(cluster => cluster
        .WithBrokers(config[Endpoints.KafkaHermesBootstrapServers])
        .WithSecurityInformation(info => info.SecurityProtocol = SecurityProtocol.Ssl)
        .ConfigureChangesManager(changes => changes
            .WithTopicPrefixToExclude(config.GetSection(TopicPattern.ToExclude).Get<string[]>()))
        .AddTopicsJsonFile($"topicsettings.hermes.{env}.json")
    ));

#endregion

#region CobWeb Cluster

services.AddKafkaCurator(kafka => kafka
    .UseConsoleLog()
    .AddCluster(cluster => cluster
        .WithBrokers(config[Endpoints.KafkaCobWebBootstrapServers])
        .WithSecurityInformation(info => info.SecurityProtocol = SecurityProtocol.Ssl)
        .ConfigureChangesManager(changes => changes
            .WithTopicPrefixToExclude(config.GetSection(TopicPattern.ToExclude).Get<string[]>()))
        .AddTopicsJsonFile($"topicsettings.cobweb.{env}.json")
    ));

#endregion

var runConfig = new RunConfiguration
{
    DryRun = args.Contains("--dry-run")
};

var provider = services.BuildServiceProvider();
var curator = provider.CreateCurator();

return await curator.RunAsync(runConfig);