namespace LSports.Kafka.Curator.Constants
{
    public static class Endpoints
    {
        public static readonly string KafkaHermesBootstrapServers =
            "MESSAGE-BROKER:KAFKA:HERMES:BOOTSTRAP-BROKER-TLS";

        public static readonly string KafkaCobWebBootstrapServers =
            "MESSAGE-BROKER:KAFKA:COBWEB:BOOTSTRAP-BROKER-TLS";
        
        public static readonly string KafkaPlatformBootstrapServers =
            "MESSAGE-BROKER:KAFKA:PLATFORM:BOOTSTRAP-BROKER-TLS";
    }
}