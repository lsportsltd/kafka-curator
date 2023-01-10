using System;

namespace KafkaCurator.Configuration
{
    public interface IStateManagerConfigurationBuilder
    {
        IStateManagerConfigurationBuilder WithName(string name);
        IStateManagerConfigurationBuilder WithTimeout(TimeSpan timeout);
    }
}