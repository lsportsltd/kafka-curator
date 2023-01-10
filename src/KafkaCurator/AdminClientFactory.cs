using System.Collections.Generic;
using Confluent.Kafka;

namespace KafkaCurator
{
    public class AdminClientFactory : IAdminClientFactory
    {
        private readonly Dictionary<string, IAdminClient> _clients = new();

        public IAdminClient GetOrCreate(string key, AdminClientConfig config)
        {
            if (_clients.TryGetValue(key, out var client)) return client;

            return _clients[key] = new AdminClientBuilder(config).Build();
        }
    }
}