namespace KafkaCurator.Changes
{
    public interface IExistingTopicsHandlerAccessor
    {
        public IExistingTopicsHandler GetExistingTopicsHandler(string name);
    }
}