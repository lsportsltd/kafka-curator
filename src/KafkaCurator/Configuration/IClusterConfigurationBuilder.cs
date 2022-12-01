using System;
using System.Collections.Generic;
using KafkaCurator.Abstractions;
using KafkaCurator.Abstractions.Configuration;

namespace KafkaCurator.Configuration
{
    public interface IClusterConfigurationBuilder
    {
        IClusterConfigurationBuilder WithBrokers(IEnumerable<string> brokers);
        IClusterConfigurationBuilder WithBrokers(string brokers);
        IClusterConfigurationBuilder WithName(string name);
        IClusterConfigurationBuilder WithSecurityInformation(Action<SecurityInformation> handler);
        IClusterConfigurationBuilder ConfigureChangesManager(Action<IChangesConfigurationBuilder> changesManager);
        IClusterConfigurationBuilder AddTopic(Action<ITopicConfigurationBuilder> topic);
        IClusterConfigurationBuilder AddTopicsJsonFile(string path);
        IClusterConfigurationBuilder UseStateConfigurator(Func<IDependencyResolver, IStateConfigurator> configurator);
    }
}