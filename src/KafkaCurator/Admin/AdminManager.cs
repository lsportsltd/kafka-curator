using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace KafkaCurator.Admin
{
    internal class AdminManager : IAdminManager
    {
        private readonly ILogger<AdminManager> _logger;
        private readonly IAdminClient _adminClient;

        public AdminManager(ILogger<AdminManager> logger, IAdminClient adminClient)
        {
            _logger = logger;
            _adminClient = adminClient;
        }
    }
}