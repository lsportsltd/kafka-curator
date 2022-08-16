using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaCurator.Kafka.Interfaces;
using KafkaCurator.Models;

namespace KafkaCurator.Kafka
{
    public class KafkaClient : IKafkaClient
    {
        private readonly IAdminClient _adminClient;

        public KafkaClient(string bootstrapServers)
        {
            _adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers, SecurityProtocol = SecurityProtocol.Ssl }).Build();
        }

        public Dictionary<string, TopicMetadata> GetKafkaTopics()
        {
            return _adminClient.GetMetadata(TimeSpan.FromSeconds(30)).Topics.Where(t => !t.Topic.StartsWith("__"))
                .ToDictionary(t => t.Topic, t => t);
        }

        public Task DeleteTopicAsync(string[] topics)
        {
            return _adminClient.DeleteTopicsAsync(topics);
        }

        public Task AlterTopicPartitionAsync(Topic topic)
        {
            var partitionsSpecifications = new List<PartitionsSpecification>();

            var partitionSpecification = new PartitionsSpecification
            {
                IncreaseTo = topic.NumOfPartitions,
                Topic = topic.Name
            };

            partitionsSpecifications.Add(partitionSpecification);

            return _adminClient.CreatePartitionsAsync(partitionsSpecifications);
        }

        public Task AlterTopicPartitionsAsync(IEnumerable<Topic> topics)
        {
            var partitionsSpecifications = new List<PartitionsSpecification>();

            foreach (var topic in topics)
            {
                var partitionSpecification = new PartitionsSpecification
                {
                    IncreaseTo = topic.NumOfPartitions,
                    Topic = topic.Name
                };

                partitionsSpecifications.Add(partitionSpecification);
            }

            return _adminClient.CreatePartitionsAsync(partitionsSpecifications);
        }

        public async Task AlterTopics(IEnumerable<(Topic, AlterTopicInfo)> alters)
        {
            foreach (var (topic, alterInfo) in alters)
            {
                if (alterInfo.ShouldAlterNumOfPartitions)
                {
                    await AlterTopicPartitionAsync(topic);
                }

                if (alterInfo.ShouldAlterCleanupPolicy)
                {
                    await AlterCleanupPolicy(topic);
                }
            }
        }

        public Task CreateTopicsAsync(IEnumerable<Topic> topics)
        {
            var newTopics = new List<TopicSpecification>();

            foreach (var topic in topics)
            {
                var topicSpecs = new TopicSpecification
                {
                    Name = topic.Name,
                    NumPartitions = topic.NumOfPartitions,
                    ReplicationFactor = topic.ReplicationFactor,
                    Configs = new Dictionary<string, string> { { "cleanup.policy", topic.CleanupPolicy.ToString().ToLower() } }
                };

                newTopics.Add(topicSpecs);
            }

            return _adminClient.CreateTopicsAsync(newTopics);
        }

        public async Task<DescribeConfigsResult> DescribeTopicConfigAsync(string topic)
        {
            var configResource = new ConfigResource
            {
                Name = topic,
                Type = ResourceType.Topic
            };

            var result = await _adminClient.DescribeConfigsAsync(new List<ConfigResource> { configResource },
                new DescribeConfigsOptions { RequestTimeout = TimeSpan.FromSeconds(30) });

            return result.FirstOrDefault();
        }

        private Task AlterCleanupPolicy(Topic topic)
        {
            var configs = GetCleanupPolicyConfigs(topic);

            return _adminClient.AlterConfigsAsync(configs,
                new AlterConfigsOptions { RequestTimeout = TimeSpan.FromSeconds(30) });
        }

        private Dictionary<ConfigResource, List<ConfigEntry>> GetCleanupPolicyConfigs(Topic topic)
        {
            var configResource = new ConfigResource
            {
                Name = topic.Name,
                Type = ResourceType.Topic
            };

            var configEntry = new ConfigEntry
            {
                Name = "cleanup.policy",
                Value = topic.CleanupPolicy.ToString().ToLower()
            };

            var dictionary = new Dictionary<ConfigResource, List<ConfigEntry>>
            {
                { configResource, new List<ConfigEntry> { configEntry } }
            };

            return dictionary;
        }
    }
}