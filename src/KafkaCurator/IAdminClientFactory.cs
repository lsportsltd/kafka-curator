using Confluent.Kafka;

namespace KafkaCurator
{
    public interface IAdminClientFactory
    {
        public IAdminClient GetOrCreate(string key, AdminClientConfig config);
    }
}