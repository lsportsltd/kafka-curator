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
        /// <param name="stopCancellationToken">A <see cref="T:System.Threading.CancellationToken" /> used to stop the operation.</param>
        /// <returns></returns>
        Task ExecuteAsync(CancellationToken stopCancellationToken = default);
    }
}