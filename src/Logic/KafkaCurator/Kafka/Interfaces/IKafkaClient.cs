using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KafkaCurator.Models;

namespace KafkaCurator.Kafka.Interfaces
{
    public interface IKafkaClient
    {
        Dictionary<string, TopicMetadata> GetKafkaTopics();
        Task DeleteTopicAsync(string[] topics);
        Task AlterTopicPartitionAsync(Topic topic);
        Task AlterTopicPartitionsAsync(IEnumerable<Topic> topics);
        Task AlterTopics(IEnumerable<(Topic, AlterTopicInfo)> alters);
        Task CreateTopicsAsync(IEnumerable<Topic> topics);
        Task<DescribeConfigsResult> DescribeTopicConfigAsync(string topic);
    }
}