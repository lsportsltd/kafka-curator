using System.Text.Json;

namespace KafkaCurator
{
    public class DefaultTopicsFileSerializer : ITopicsFileSerializer
    {
        public TopicsFile Deserialize(byte[] payload) =>
            JsonSerializer.Deserialize<TopicsFile>(payload);
    }
}