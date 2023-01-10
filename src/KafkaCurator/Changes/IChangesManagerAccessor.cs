namespace KafkaCurator.Changes
{
    public interface IChangesManagerAccessor
    {
        IChangesManager GetChangesManager(string name);
    }
}