using Confluent.Kafka;

namespace KafkaCurator.Configuration
{
    public class AdminConfiguration : IAdminConfiguration
    {
        private readonly AdminClientConfig _adminClientConfig;

        public AdminConfiguration(AdminClientConfig adminClientConfig)
        {
            _adminClientConfig = adminClientConfig;
        }
    }
}