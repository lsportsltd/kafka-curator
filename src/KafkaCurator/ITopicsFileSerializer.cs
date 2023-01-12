namespace KafkaCurator
{
    public interface ITopicsFileSerializer
    {
        TopicsFile Deserialize(byte[] payload);
    }
}