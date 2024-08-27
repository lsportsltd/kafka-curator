using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaCurator.Configuration;
using KafkaCurator.Extensions;

namespace KafkaCurator.State
{
    public class DefaultStateManager : StateManagerBase<EmptyStateParameters>
    {
        private readonly IAdminClient _adminClient;
        private readonly StateManagerConfiguration _configuration;

        public DefaultStateManager(IAdminClient adminClient, StateManagerConfiguration configuration)
        {
            _adminClient = adminClient;
            _configuration = configuration;
        }

        public override Task SetState(IReadOnlyList<ITopicConfiguration> topicConfigurations)
        {
            return Task.CompletedTask;
        }

        protected override Task<IReadOnlyList<ITopicConfiguration>> GetStateInternal(EmptyStateParameters stateParameters)
        {
            return GetTopicMetadata();
        }
        
        private async Task<IReadOnlyList<ITopicConfiguration>> GetTopicMetadata()
        {
            var topicsMetadata = _adminClient.GetMetadata(_configuration.Timeout).Topics
                .ToDictionary(t => t.Topic, t => t);
            if (topicsMetadata.Keys.Count == 0) return new List<ITopicConfiguration>().AsReadOnly();
            
            var topicsConfigResults = await DescribeTopicConfigAsync(topicsMetadata.Keys);

            var topicConfigurations = new List<ITopicConfiguration>();
            
            foreach (var describeConfigsResult in topicsConfigResults)
            {
                if(!topicsMetadata.TryGetValue(describeConfigsResult.ConfigResource.Name, out var metadata)) continue;
                topicConfigurations.Add(metadata.ToTopicConfiguration(describeConfigsResult));
            }

            return topicConfigurations.AsReadOnly();
        }

        private async Task<List<DescribeConfigsResult>> DescribeTopicConfigAsync(IEnumerable<string> topics)
        {
            var configResourceList = new List<ConfigResource>();

            foreach (var topic in topics)
            {
                var configResource = new ConfigResource
                {
                    Name = topic,
                    Type = ResourceType.Topic
                };
                
                configResourceList.Add(configResource);
            }
            
            var result = await _adminClient.DescribeConfigsAsync(configResourceList,
                new DescribeConfigsOptions {RequestTimeout = _configuration.Timeout});

            return result;
        }

    }
}