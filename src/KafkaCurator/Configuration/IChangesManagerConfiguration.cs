using System;
using System.Collections.Generic;
using Confluent.Kafka;

namespace KafkaCurator.Configuration
{
    public interface IChangesManagerConfiguration
    {
        string ClusterName { get; }
        AdminClientConfig AdminClientConfig { get; }
        IReadOnlyList<string> PrefixesToExclude { get; }
        TimeSpan Timeout { get; }
    }
}