using System.Threading.Tasks;
using KafkaCurator.Abstractions;
using KafkaCurator.Configuration;

namespace KafkaCurator.Changes
{
    public class DeleteTopicsHandler : TopicsHandlerBase, IDeleteTopicsHandler
    {
        string IDeleteTopicsHandler.Name => _configuration.ClusterName;
        
        private readonly ChangesManagerConfiguration _configuration;
        private readonly ILogHandler _logHandler;

        public DeleteTopicsHandler(IDependencyResolver resolver, ChangesManagerConfiguration configuration) : base(resolver, configuration)
        {
            _configuration = configuration;
            _logHandler = resolver.Resolve<ILogHandler>();
        }

        public int PreviewDeleteTopics(string[] topics)
        {
            if (!SanityCheck(topics)) return 0;

            PrintTopicsToDelete(topics);
            return topics.Length;
        }

        public async Task<int> HandleDeleteTopics(string[] topics)
        {
            if (!SanityCheck(topics)) return 0;
            
            PrintTopicsToDelete(topics);
            await AdminClient.DeleteTopicsAsync(topics);

            return topics.Length;
        }

        private bool SanityCheck(string[] topics)
        {
            if (topics.Length == 0)
            {
                _logHandler.Info("    There are no topics to delete.");
                return false;
            }

            return true;
        }

        private void PrintTopicsToDelete(string[] topics)
        {
            foreach (var topic in topics)
            {
                _logHandler.Info($"  - {topic}");
            }
        }
    }
}