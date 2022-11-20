using System;

namespace KafkaCurator.Core.Kafka
{
    public class KafkaClientOptions
    {
        public string BootstrapServers { get; set; }

        public void SanityCheck()
        {
            if (string.IsNullOrEmpty(BootstrapServers)) throw new ArgumentNullException(nameof(BootstrapServers));
        }
    }
}