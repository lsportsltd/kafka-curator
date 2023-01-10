using System;

namespace KafkaCurator.Configuration
{
    public class StateManagerConfiguration
    {
        public string Name { get; }
        public TimeSpan Timeout { get; }
        public Type Type { get; }

        public StateManagerConfiguration(string name, TimeSpan timeout, Type type)
        {
            Name = name ?? Guid.NewGuid().ToString();
            Timeout = timeout;
            Type = type;
        }
    }
}