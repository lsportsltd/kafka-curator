using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaCurator.Core.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KafkaCurator.Core.Kafka
{
    public class KafkaClient : IKafkaClient
    {
        private readonly ILogger<IKafkaClient> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAdminClient _adminClient;

        public KafkaClient(KafkaClientOptions clientOptions, ILogger<IKafkaClient> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = clientOptions.BootstrapServers, SecurityProtocol = SecurityProtocol.Ssl }).Build();
        }

        public Dictionary<string, TopicMetadata> GetKafkaTopics()
        {
            var pattensToExclude = _configuration.GetSection(TopicPattern.ToExclude).Get<string[]>();
            var topics = _adminClient.GetMetadata(TimeSpan.FromSeconds(30)).Topics;
            var relevantTopics = topics.Where(t => pattensToExclude.All(pattern => !t.Topic.StartsWith(pattern)));

            return relevantTopics.ToDictionary(t => t.Topic, t => t);
        }

        public Task DeleteTopicAsync(string[] topics)
        {
            _logger.LogInformation($"Deleting the following topics: {string.Join(',', topics)}");
            return _adminClient.DeleteTopicsAsync(topics);
        }

        public Task AlterTopicPartitionAsync(Topic topic)
        {
            _logger.LogInformation($"Altering num of partitions for topic: {topic.Name}, num of partitions: {topic.NumOfPartitions}");

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
            _logger.LogInformation($"Altering cleanup.policy for topic: {topic.Name}, new policy: {topic.CleanupPolicy.ToString().ToLower()}");

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