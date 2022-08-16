using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace KafkaCurator.Core.Kafka
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