using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaCurator.Configuration;

namespace KafkaCurator
{
    public class DefaultStateHandler : IStateHandler
    {
        private readonly IAdminClient _adminClient;
        private readonly TimeSpan _timeout;

        public DefaultStateHandler(IAdminClient adminClient, TimeSpan timeout)
        {
            _adminClient = adminClient;
            _timeout = timeout;
        }
        
        public List<ITopicConfiguration> GetState()
        {
            var topicsMetadata = GetTopicMetadata();
        }

        public Task<List<ITopicConfiguration>> GetStateAsync()
        {
            throw new System.NotImplementedException();
        }

        public void SetState(IEnumerable<ITopicConfiguration> state)
        {
            throw new System.NotImplementedException();
        }

        public Task SetStateAsync(IEnumerable<ITopicConfiguration> state)
        {
            throw new System.NotImplementedException();
        }

        private List<TopicMetadata> GetTopicMetadata() 
    }
}