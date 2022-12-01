using System;
using System.Collections.Generic;
using Confluent.Kafka;

namespace KafkaCurator.Configuration
{
    public class ChangesManagerConfiguration : IChangesManagerConfiguration
    {
        public string ClusterName { get; }
        public AdminClientConfig AdminClientConfig { get; }
        public IReadOnlyList<string> PrefixesToExclude { get; }
        public TimeSpan Timeout { get; }

        public ChangesManagerConfiguration(string clusterName, AdminClientConfig adminClientConfig, IReadOnlyList<string> prefixesToExclude, TimeSpan timeout)
        {
            ClusterName = clusterName;
            AdminClientConfig = adminClientConfig;
            PrefixesToExclude = prefixesToExclude;
            Timeout = timeout;
        }
    }
}