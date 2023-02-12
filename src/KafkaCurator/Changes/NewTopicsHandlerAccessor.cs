using System.Collections.Generic;
using System.Linq;

namespace KafkaCurator.Changes
{
    public class NewTopicsHandlerAccessor : INewTopicsHandlerAccessor
    {
        private readonly Dictionary<string, INewTopicsHandler> _handlers;

        public NewTopicsHandlerAccessor(IEnumerable<INewTopicsHandler> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.Name, h => h);
        }

        public INewTopicsHandler GetNewTopicsHandler(string name) => _handlers.TryGetValue(name, out var handler) ? handler : null;
    }
}