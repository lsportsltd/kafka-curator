using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using KafkaCurator.Abstractions;
using KafkaCurator.Abstractions.Extensions;
using KafkaCurator.Changes;
using KafkaCurator.Extensions;

namespace KafkaCurator.Configuration
{
    public class KafkaConfigurationBuilder : IKafkaConfigurationBuilder
    {
        public IDependencyConfigurator DependencyConfigurator { get; }

        private readonly List<ClusterConfigurationBuilder> _clusters = new();

        private Type _logHandlerType = typeof(NullLogHandler);

        public KafkaConfigurationBuilder(IDependencyConfigurator dependencyConfigurator)
        {
            DependencyConfigurator = dependencyConfigurator;
        }

        public IKafkaConfigurationBuilder AddCluster(Action<IClusterConfigurationBuilder> cluster)
        {
            var builder = new ClusterConfigurationBuilder(DependencyConfigurator);

            cluster(builder);

            _clusters.Add(builder);

            return this;
        }

        public IKafkaConfigurationBuilder UseLogHandler<TLogHandler>()
            where TLogHandler : ILogHandler
        {
            _logHandlerType = typeof(TLogHandler);
            return this;
        }

        public KafkaConfiguration Build()
        {
            var configuration = new KafkaConfiguration();

            configuration.AddClusters(_clusters.Select(c => c.Build(configuration)));

            DependencyConfigurator
                .AddSingleton<IChangesManagerFactory, ChangesManagerFactory>()
                .AddSingleton<IAdminClientFactory, AdminClientFactory>()
                .AddTopicAlterServices()
                .AddSingleton<IChangesManagerAccessor>(resolver =>
                    new ChangesManagerAccessor(configuration.Clusters.Select(c => c.ChangesManager)
                        .Select(cmc => new ChangesManager(resolver, cmc))))
                .AddSingleton<IExistingTopicsHandlerAccessor>(resolver =>
                    new ExistingTopicsHandlerAccessor(configuration.Clusters.Select(c => c.ChangesManager)
                        .Select(cmc => new ExistingTopicsHandler(resolver, cmc))))
                .AddSingleton<INewTopicsHandlerAccessor>(resolver =>
                    new NewTopicsHandlerAccessor(configuration.Clusters.Select(c => c.ChangesManager)
                        .Select(cmc => new NewTopicsHandler(resolver, cmc))))
                .AddSingleton<IDeleteTopicsHandlerAccessor>(resolver =>
                    new DeleteTopicsHandlerAccessor(configuration.Clusters.Select(c => c.ChangesManager)
                        .Select(cmc => new DeleteTopicsHandler(resolver, cmc))))
                .AddTransient(typeof(ILogHandler), _logHandlerType);

            return configuration;
        }
    }
}