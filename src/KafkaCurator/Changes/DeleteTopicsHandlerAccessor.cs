using System.Collections.Generic;
using System.Linq;

namespace KafkaCurator.Changes
{
    public class DeleteTopicsHandlerAccessor : IDeleteTopicsHandlerAccessor
    {
        private readonly Dictionary<string, IDeleteTopicsHandler> _handlers;

        public DeleteTopicsHandlerAccessor(IEnumerable<IDeleteTopicsHandler> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.Name, h => h);
        }

        public IDeleteTopicsHandler GetDeleteHandler(string name) => _handlers.TryGetValue(name, out var handler) ? handler : null;
    }
}