using KafkaCurator.Extensions.Microsoft.DependencyInjection;
using KafkaCurator.LogHandler.Console;
using LSports.Extensions.Kafka.Curator;
using LSports.Kafka.Curator;
using LSports.Kafka.Curator.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        .WithBrokers(config[Endpoints.KafkaBootstrapServers])
        .WithSecurityInformation(info => info.SecurityProtocol = SecurityProtocol.Ssl)
        .ConfigureChangesManager(changes => changes
            .WithTopicPrefixToExclude(config.GetSection(TopicPattern.ToExclude).Get<string[]>()))
        .AddTopicsJsonFile($"topicsettings.{env}.json")
        .WithCustomTopicsFileSerializer(new CustomTopicsFileSerializer())
    ));

var provider = services.BuildServiceProvider();
var curator = provider.CreateCurator();

return await curator.ExecuteAsync();