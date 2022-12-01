using System;
using System.Collections.Generic;
using Confluent.Kafka;

namespace KafkaCurator.Configuration
{
    public interface IChangesConfigurationBuilder
    {
        IChangesConfigurationBuilder WithAdminConfig(AdminClientConfig config);
        IChangesConfigurationBuilder WithTopicPrefixToExclude(string prefix);
        IChangesConfigurationBuilder WithTopicPrefixToExclude(IEnumerable<string> prefixes);
        IChangesConfigurationBuilder WithTimeout(TimeSpan timeout);
    }
}