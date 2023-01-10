using System.Threading;
using System.Threading.Tasks;

namespace KafkaCurator
{
    /// <summary>
    /// Provides access to the kafka curator operations
    /// </summary>
    public interface IKafkaCurator
    {
        /// <summary>
        /// Executes Kafka Curator
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> used to stop the operation.</param>
        /// <returns></returns>
        Task<int> ExecuteAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Preview Kafka Curator changes
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> used to stop the operation.</param>
        /// <returns></returns>
        Task<int> PreviewAsync(CancellationToken cancellationToken = default);
    }
}