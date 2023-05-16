using KafkaCurator.Configuration;
using LSports.Kafka.Curator.Constants;
using KafkaCurator.LogHandler.Console;
using LSports.Kafka.Curator.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KafkaCurator.Extensions.Microsoft.DependencyInjection;
using SecurityProtocol = KafkaCurator.Abstractions.Configuration.SecurityProtocol;

var env = Environment.GetEnvironmentVariable(EnvironmentVariableName.HostEnvironment);

var config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json", false)
    .AddAwsSsm()
    .Build();

var services = new ServiceCollection();

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

var provider = services.BuildServiceProvider();
var curator = provider.CreateCurator();

var runConfig = new RunConfiguration
{
    DryRun = args.Contains("--dry-run")
};

return await curator.RunAsync(runConfig);