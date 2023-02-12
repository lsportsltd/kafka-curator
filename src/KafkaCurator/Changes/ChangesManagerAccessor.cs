using System.Collections.Generic;
using System.Linq;

namespace KafkaCurator.Changes
{
    public class ChangesManagerAccessor : IChangesManagerAccessor
    {
        private readonly Dictionary<string, IChangesManager> _changesManagers;

        public ChangesManagerAccessor(IEnumerable<IChangesManager> changesManagers)
        {
            _changesManagers = changesManagers.ToDictionary(cm => cm.Name, cm => cm);
        }

        public IChangesManager GetChangesManager(string name) =>
            _changesManagers.TryGetValue(name, out var changesManager) ? changesManager : null;
    }

    public interface IExistingTopicsHandlerAccessor
    {
        public IExistingTopicsHandler GetExistingTopicsHandler(string name);
    }

    public class ExistingTopicsHandlerAccessor : IExistingTopicsHandlerAccessor
    {
        private readonly Dictionary<string, IExistingTopicsHandler> _handlers;

        public ExistingTopicsHandlerAccessor(IEnumerable<IExistingTopicsHandler> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.Name, h => h);
        }

        public IExistingTopicsHandler GetExistingTopicsHandler(string name) => _handlers.TryGetValue(name, out var handler) ? handler : null;
    }

    public interface INewTopicsHandlerAccessor
    {
        INewTopicsHandler GetNewTopicsHandler(string name);
    }

    public class NewTopicsHandlerAccessor : INewTopicsHandlerAccessor
    {
        private readonly Dictionary<string, INewTopicsHandler> _handlers;

        public NewTopicsHandlerAccessor(IEnumerable<INewTopicsHandler> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.Name, h => h);
        }

        public INewTopicsHandler GetNewTopicsHandler(string name) => _handlers.TryGetValue(name, out var handler) ? handler : null;
    }

    public interface IDeleteTopicsHandlerAccessor
    {
        IDeleteTopicsHandler GetDeleteHandler(string name);
    }

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