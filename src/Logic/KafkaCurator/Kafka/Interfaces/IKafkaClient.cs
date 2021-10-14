using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaCurator.Models;

namespace KafkaCurator.Kafka.Interfaces
{
    public interface IKafkaClient
    {
        Dictionary<string, TopicMetadata> GetKafkaTopics();
        Task DeleteTopicAsync(string[] topics);
        Task AlterTopicPartitionsAsync(IEnumerable<Topic> topics);
        Task CreateTopicsAsync(IEnumerable<Topic> topics);
    }
}