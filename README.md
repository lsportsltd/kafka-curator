[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/nizanrosh/kafka-curator/blob/main/LICENSE) [![nuget version](https://img.shields.io/nuget/v/KafkaCurator.svg?style=flat)](https://www.nuget.org/packages/KafkaCurator) [![Build](https://github.com/nizanrosh/kafka-curator/actions/workflows/build.yml/badge.svg)](https://github.com/nizanrosh/kafka-curator/actions/workflows/build.yml)
# kafka-curator

# Introduction
- KafkaCurator was designed to provide a simple and supervised way to manage everything related to Kafka topics.
- A lot of the code in this library was inspired by [KafkaFlow](https://github.com/Farfetch/kafkaflow).

# Getting Started
## Requirements
- .NET Core 2.1 or above.
- .NET Framework 4.6.1 or above.

## Installation
Install the required Nuget Packages:
- [KafkaCurator](https://www.nuget.org/packages/KafkaCurator)
- [KafkaCurator.Microsoft.DependencyInjection](https://www.nuget.org/packages/KafkaCurator.Microsoft.DependencyInjection)
- [KafkaCurator.LogHandler.Console](https://www.nuget.org/packages/KafkaCurator.LogHandler.Console)

## Setup

Before you is a simple console application which runs the curator in order to create a set of topics configured within the `topics.json` file.
For the complete project, feel free to view the [Samples](https://github.com/nizanrosh/kafka-curator/tree/main/samples/) folder.

```cs
using Confluent.Kafka;
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

return await curator.ExecuteAsync();
```

# Topic Configuration
All topic configurations are support with the exception of [Replication Factor](https://docs.confluent.io/kafka-connectors/firebase/current/source/firebase_source_connector_config.html#cp-license:~:text=confluent.topic.replication,brokers%20(often%201).) which will be supported later on.

Below is a list of the configurations:\
`CleanupPolicy`\
`CompressionType`\
`MessageDownConversionEnable`\
`MinInSyncReplicas`\
`SegmentJitterMs`\
`FlushMs`\
`FollowerReplicationThrottledReplicas`\
`SegmentBytes`\
`RetentionMs`\
`FlushMessages`\
`MessageFormatVersion`\
`MaxCompactionLagMs`\
`MaxMessageBytes`\
`MinCompactionLagMs`\
`MessageTimestampType`\
`Preallocate`\
`MinCleanableDirtyRation`\
`IndexIntervalBytes`\
`UncleanLeaderElectionEnable`\
`RetentionBytes`\
`DeleteRetentionMs`\
`SegmentMs`\
`MessageTimestampDifferenceMaxMs`\
`SegmentIndexBytes`\
`FileDeleteDelayMs`

Each of the configuration entries shown above have its own method to be specified inline:
```cs
services.AddKafkaCurator(kafka => kafka
    .UseConsoleLog()
    .AddCluster(cluster => cluster
        .WithBrokers("localhost:9092")
        .AddTopic(topic => topic
            .WithMinCompactionLagMs(10)
            .WithMinInSyncReplicas(3)
            .WithPreallocate(false)
        )
    ));
```

Alternatively, you can specify all configuration within a json file and then add that json file to the curator and only update that file:

```json
{
    "Topics": [
        {
            "Name": "Rick.Sanchez",
            "ReplicationFactor": 3,
            "Partitions": 10,
            "CleanupPolicy": "compact",
            "MaxMessageBytes": "10485880"
        },
        {
            "Name": "Morty.Smith",
            "ReplicationFactor": 3,
            "Partitions": 3,
            "CleanupPolicy": "delete",
            "Preallocate": false
        }
    ]
}
```

And then add that file to the curator:
```cs
services.AddKafkaCurator(kafka => kafka
    .UseConsoleLog()
    .AddCluster(cluster => cluster
        .WithBrokers("localhost:9092")
        .AddTopicsJsonFile("topics.json")
    ));
```

# License
kafka-curator is a free and open source project, released under the permissible [MIT License](https://github.com/nizanrosh/kafka-curator/blob/main/LICENSE)
