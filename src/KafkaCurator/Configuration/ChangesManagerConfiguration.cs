using System;
using System.Collections.Generic;
using Confluent.Kafka;

namespace KafkaCurator.Configuration
{
    public class ChangesManagerConfiguration
    {
        public string ClusterName { get; }
        public AdminClientConfig AdminClientConfig { get; }
        public IReadOnlyList<string> PrefixesToExclude { get; }
        public TimeSpan Timeout { get; }
        public StateManagerConfiguration StateManagerConfiguration { get; }

        public ChangesManagerConfiguration(string clusterName, AdminClientConfig adminClientConfig,
            IReadOnlyList<string> prefixesToExclude, TimeSpan timeout,
            StateManagerConfiguration stateManagerConfiguration)
        {
            ClusterName = clusterName;
            AdminClientConfig = adminClientConfig;
            PrefixesToExclude = prefixesToExclude;
            Timeout = timeout;
            StateManagerConfiguration = stateManagerConfiguration;
        }
    }
}