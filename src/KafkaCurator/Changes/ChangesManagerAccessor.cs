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
}