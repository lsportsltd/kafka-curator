using System.Threading;
using System.Threading.Tasks;
using KafkaCurator.Configuration;

namespace KafkaCurator
{
    /// <summary>
    /// Provides access to the kafka curator operations
    /// </summary>
    public interface IKafkaCurator
    {
        /// <summary>
        /// Executes Kafka Curator.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> used to stop the operation.</param>
        /// <returns></returns>
        Task<int> ExecuteAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Preview Kafka Curator changes.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> used to stop the operation.</param>
        /// <returns></returns>
        Task<int> PreviewAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Preview and then execute Kafka Curator.
        /// </summary>
        /// <param name="runConfiguration">A <see cref="T:KafkaCurator.Configuration" /> used to stop the operation.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> used to stop the operation.</param>
        /// <returns></returns>
        Task<int> RunAsync(RunConfiguration runConfiguration, CancellationToken cancellationToken = default);
    }
}