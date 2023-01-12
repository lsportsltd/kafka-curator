using System.Text;
using KafkaCurator;
using Newtonsoft.Json;

namespace LSports.Kafka.Curator;

public class CustomTopicsFileSerializer : ITopicsFileSerializer
{
    public TopicsFile Deserialize(byte[] payload)
    {
        var json = Encoding.UTF8.GetString(payload);
        return JsonConvert.DeserializeObject<TopicsFile>(json);
    }
}