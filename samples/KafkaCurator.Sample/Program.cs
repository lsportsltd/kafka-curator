using Confluent.Kafka;
using KafkaCurator.Configuration;
using KafkaCurator.Extensions.Microsoft.DependencyInjection;
using KafkaCurator.LogHandler.Console;
using Microsoft.Extensions.DependencyInjection;
using SecurityProtocol = KafkaCurator.Abstractions.Configuration.SecurityProtocol;

var services = new ServiceCollection();

services.AddKafkaCurator(kafka => kafka
    .UseConsoleLog()
    .AddCluster(cluster => cluster
        .WithBrokers("localhost:9094")
        .WithSecurityInformation(information => information.SecurityProtocol = SecurityProtocol.Ssl)
        .ConfigureChangesManager(changesManager => changesManager
            .WithAdminConfig(new AdminClientConfig())
            .WithTopicPrefixToExclude("__")
            .WithTopicPrefixToExclude("kafka-s3-sink")
            .WithTimeout(TimeSpan.FromSeconds(15)))
        .AddTopicsJsonFile("topics.json")
    ));

var provider = services.BuildServiceProvider();

var curator = provider.CreateCurator();

return await curator.RunAsync(new RunConfiguration());