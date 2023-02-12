namespace KafkaCurator.Changes
{
    public interface INewTopicsHandlerAccessor
    {
        INewTopicsHandler GetNewTopicsHandler(string name);
    }
}