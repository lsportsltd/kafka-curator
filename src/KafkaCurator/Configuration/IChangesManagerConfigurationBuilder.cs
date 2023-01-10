using System;
using System.Collections.Generic;
using Confluent.Kafka;
using KafkaCurator.Changes;

namespace KafkaCurator.Configuration
{
    public interface IChangesManagerConfigurationBuilder
    {
        IChangesManagerConfigurationBuilder WithAdminConfig(AdminClientConfig config);
        IChangesManagerConfigurationBuilder WithTopicPrefixToExclude(string prefix);
        IChangesManagerConfigurationBuilder WithTopicPrefixToExclude(IEnumerable<string> prefixes);
        IChangesManagerConfigurationBuilder WithTopicPrefixToExclude(params string[] prefixes);
        IChangesManagerConfigurationBuilder WithTimeout(TimeSpan timeout);
        IChangesManagerConfigurationBuilder UseStateManager<TStateManager>(
            Action<IStateManagerConfigurationBuilder> stateManager) where TStateManager : class, IStateManager;
    }
}