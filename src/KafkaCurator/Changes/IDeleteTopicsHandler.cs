using System.Threading.Tasks;

namespace KafkaCurator.Changes
{
    public interface IDeleteTopicsHandler
    {
        internal string Name { get; }
        int PreviewDeleteTopics(string[] topics);
        Task<int> HandleDeleteTopics(string[] topics);
    }
}