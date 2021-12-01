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
    }
}