using System;
using Confluent.Kafka;
using KafkaCurator.Abstractions;

namespace KafkaCurator
{
    public class DefaultStateConfigurator : IStateConfigurator
    {
        private readonly IAdminClient _adminClient;
        private readonly TimeSpan _timeout;

        public DefaultStateConfigurator(IAdminClient adminClient, TimeSpan timeout)
        {
            _adminClient = adminClient;
            _timeout = timeout;
        }
        
        public IStateHandler CreateStateHandler(IDependencyResolver resolver)
        {
            return new DefaultStateHandler(_adminClient, _timeout);
        }
    }
}