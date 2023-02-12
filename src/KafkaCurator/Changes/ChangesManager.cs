using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KafkaCurator.Abstractions;
using KafkaCurator.Configuration;
using KafkaCurator.State;

namespace KafkaCurator.Changes
{
    internal class ChangesManager : IChangesManager
    {
        public string Name => _changesManagerConfiguration.ClusterName;

        private readonly ILogHandler _logHandler;
        private readonly ChangesManagerConfiguration _changesManagerConfiguration;

        private readonly IStateManager _stateManager;
        private readonly IExistingTopicsHandler _existingTopicsHandler;
        private readonly INewTopicsHandler _newTopicsHandler;
        private readonly IDeleteTopicsHandler _deleteTopicsHandler;

        public ChangesManager(IDependencyResolver dependencyResolver,
            ChangesManagerConfiguration changesManagerConfiguration)
        {
            _logHandler = dependencyResolver.Resolve<ILogHandler>();
            _changesManagerConfiguration = changesManagerConfiguration;

            _stateManager = GetStateManager(dependencyResolver, changesManagerConfiguration);
            _existingTopicsHandler = dependencyResolver.Resolve<IExistingTopicsHandlerAccessor>().GetExistingTopicsHandler(Name);
            _newTopicsHandler = dependencyResolver.Resolve<INewTopicsHandlerAccessor>().GetNewTopicsHandler(Name);
            _deleteTopicsHandler = dependencyResolver.Resolve<IDeleteTopicsHandlerAccessor>().GetDeleteHandler(Name);
        }

        public async Task PreviewChanges(IReadOnlyList<ITopicConfiguration> topics)
        {
            _logHandler.Info($"Previewing update ({_changesManagerConfiguration.ClusterName})...\n");
            
            var state = (await _stateManager.GetState(new EmptyStateParameters())).Where(x =>
                    _changesManagerConfiguration.PrefixesToExclude.All(prefix => x.Name.StartsWith(prefix) == false))
                .ToDictionary(t => t.Name, t => t);

            _logHandler.Info($"Found {topics.Count} topics within configuration...");
            _logHandler.Info($"Found {state.Count} topics within state...");
            
            _logHandler.Info("@ Previewing existing topics...");
            var existingTopics = Intersect(topics, state).ToArray();
            var numOfTopicsToUpdate = _existingTopicsHandler.PreviewExistingTopics(existingTopics);
            _logHandler.Info("Done.");
            
            _logHandler.Info("@ Previewing new topics...");
            var newTopics = topics.Where(t => !state.ContainsKey(t.Name)).ToArray();
            var numOfTopicsToCreate = _newTopicsHandler.PreviewNewTopics(newTopics);
            _logHandler.Info("Done.");
            
            _logHandler.Info("@ Previewing topics to delete...");
            var topicsToDelete = state.Keys.Except(topics.Select(t => t.Name)).ToArray();
            var numOfTopicsToDelete = _deleteTopicsHandler.PreviewDeleteTopics(topicsToDelete);
            _logHandler.Info("Done.\n");

            SummarizePreview(numOfTopicsToUpdate, numOfTopicsToCreate, numOfTopicsToDelete, topics.Count);
        }

        public async Task HandleChanges(IReadOnlyList<ITopicConfiguration> topics)
        {
            _logHandler.Info($"Updating ({_changesManagerConfiguration.ClusterName})...\n");
            
            var state = (await _stateManager.GetState(new EmptyStateParameters())).Where(x =>
                    _changesManagerConfiguration.PrefixesToExclude.All(prefix => x.Name.StartsWith(prefix) == false))
                .ToDictionary(t => t.Name, t => t);

            _logHandler.Info($"Found {topics.Count} topics within configuration...");
            _logHandler.Info($"Found {state.Count} topics within state...");

            _logHandler.Info("@ Handling existing topics...");
            var existingTopics = Intersect(topics, state).ToArray();
            var numOfTopicsToUpdate = await _existingTopicsHandler.HandleExistingTopics(existingTopics);
            _logHandler.Info("Done.");
            
            _logHandler.Info("@ Handling new topics...");
            var newTopics = topics.Where(t => !state.ContainsKey(t.Name)).ToArray();
            var numOfTopicsToCreate = await _newTopicsHandler.HandleNewTopics(newTopics);
            _logHandler.Info("Done.");
            
            _logHandler.Info("@ Handling topics to delete...");
            var topicsToDelete = state.Keys.Except(topics.Select(t => t.Name)).ToArray();
            var numOfTopicsToDelete = await _deleteTopicsHandler.HandleDeleteTopics(topicsToDelete);
            _logHandler.Info("Done.\n");

            _logHandler.Info("Updating state...\n");
            await _stateManager.SetState(topics);
            
            SummarizeUpdate(numOfTopicsToUpdate, numOfTopicsToCreate, numOfTopicsToDelete, topics.Count);
        }

        private IStateManager GetStateManager(IDependencyResolver dependencyResolver,
            ChangesManagerConfiguration changesManagerConfiguration)
        {
            var adminClientFactory = dependencyResolver.Resolve<IAdminClientFactory>();
            var adminClient = adminClientFactory.GetOrCreate(changesManagerConfiguration.ClusterName,
                changesManagerConfiguration.AdminClientConfig);

            if (changesManagerConfiguration.StateManagerConfiguration.Type == null)
            {
                return new DefaultStateManager(adminClient, changesManagerConfiguration.StateManagerConfiguration);
            }

            return dependencyResolver.Resolve(changesManagerConfiguration.StateManagerConfiguration.Type) as
                IStateManager;
        }

        private IEnumerable<(ITopicConfiguration, ITopicConfiguration)> Intersect(
            IReadOnlyList<ITopicConfiguration> topics, IReadOnlyDictionary<string, ITopicConfiguration> state)
        {
            foreach (var topicConfiguration in topics)
            {
                if (state.TryGetValue(topicConfiguration.Name, out var topic)) yield return (topicConfiguration, topic);
            }
        }

        private void SummarizePreview(int numToUpdate, int numToCreate, int numToDelete, int totalTopics)
        {
            _logHandler.Info("Topics:");

            if (numToUpdate == 0 && numToCreate == 0 && numToDelete == 0)
            {
                _logHandler.Info($"    {totalTopics} unchanged.\n");
                return;
            }
            
            if(numToUpdate != 0) _logHandler.Info($"  ~ {numToUpdate} to update.");
            if(numToCreate != 0) _logHandler.Info($"  + {numToCreate} to create.");
            if(numToDelete != 0) _logHandler.Info($"  - {numToDelete} to delete.");
            
            _logHandler.Info($"  {numToUpdate + numToCreate + numToDelete} changes. {totalTopics - numToCreate - numToDelete} unchanged\n");
        }
        
        private void SummarizeUpdate(int numToUpdate, int numToCreate, int numToDelete, int totalTopics)
        {
            _logHandler.Info("Topics:");

            if (numToUpdate == 0 && numToCreate == 0 && numToDelete == 0)
            {
                _logHandler.Info($"    {totalTopics} unchanged.\n");
                return;
            }
            
            if(numToUpdate != 0) _logHandler.Info($"  ~ {numToUpdate} updated.");
            if(numToCreate != 0) _logHandler.Info($"  + {numToCreate} created.");
            if(numToDelete != 0) _logHandler.Info($"  - {numToDelete} deleted.");
            
            _logHandler.Info($"  {numToUpdate + numToCreate + numToDelete} changes. {totalTopics - numToCreate - numToDelete} unchanged");
        }
    }
}