using System.Collections.Generic;
using System.Linq;

namespace KafkaCurator.Changes.TopicAltering
{
    public class TopicAlterFactory : ITopicAlterFactory
    {
        private readonly Dictionary<string, ITopicAlterHandler> _handlers;
        
        public TopicAlterFactory(IEnumerable<ITopicAlterHandler> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.ConfigEntry.ToString(), h => h);
        }

        public ITopicAlterHandler GetHandler(string configEntry) =>
            _handlers.TryGetValue(configEntry, out var handler) ? handler : null;
    }
}