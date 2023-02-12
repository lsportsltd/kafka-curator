using System.Collections.Generic;
using System.Linq;

namespace KafkaCurator.Changes
{
    public class ExistingTopicsHandlerAccessor : IExistingTopicsHandlerAccessor
    {
        private readonly Dictionary<string, IExistingTopicsHandler> _handlers;

        public ExistingTopicsHandlerAccessor(IEnumerable<IExistingTopicsHandler> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.Name, h => h);
        }

        public IExistingTopicsHandler GetExistingTopicsHandler(string name) => _handlers.TryGetValue(name, out var handler) ? handler : null;
    }
}