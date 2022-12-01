// See https://aka.ms/new-console-template for more information

using Confluent.Kafka;
using KafkaCurator.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddKafkaCurator(kafka => kafka
    .AddCluster(cluster => cluster
        .WithBrokers("")
        .ConfigureChangesManager(changesManager => changesManager
            .WithAdminConfig(new AdminClientConfig())
            .WithTopicPrefixToExclude("__")
            .WithTopicPrefixToExclude("cdc")
            .WithTimeout(TimeSpan.FromSeconds(15)))
        .AddTopic(topic => topic
            .Name("someTopic")
            .WithNumberOfPartitions(3)
        )
        .AddTopicsJsonFile("topics.json")));

var provider = services.BuildServiceProvider();

var curator = provider.CreateCurator();

await curator.ExecuteAsync();