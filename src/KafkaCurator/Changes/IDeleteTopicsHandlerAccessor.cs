namespace KafkaCurator.Changes
{
    public interface IDeleteTopicsHandlerAccessor
    {
        IDeleteTopicsHandler GetDeleteHandler(string name);
    }
}