using System;
using KafkaCurator.Abstractions;

namespace KafkaCurator.Configuration
{
    public interface IKafkaConfigurationBuilder
    {
        /// <summary>
        /// Adds a new Cluster
        /// </summary>
        /// <param name="cluster">A handle to configure the cluster</param>
        /// <returns></returns>
        IKafkaConfigurationBuilder AddCluster(Action<IClusterConfigurationBuilder> cluster);
        
        /// <summary>
        /// Set the log handler to be used by the Framework, if none is provided the <see cref="NullLogHandler"/> will be used
        /// </summary>
        /// <typeparam name="TLogHandler">A class that implements the <see cref="ILogHandler"/> interface</typeparam>
        /// <returns></returns>
        IKafkaConfigurationBuilder UseLogHandler<TLogHandler>()
            where TLogHandler : ILogHandler;
    }
}