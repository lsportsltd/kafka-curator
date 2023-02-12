using System;

namespace KafkaCurator.Abstractions
{
    /// <summary>
    /// A log handler that does nothing
    /// </summary>
    public class NullLogHandler : ILogHandler
    {
        /// <inheritdoc />
        public void Error(string message, Exception ex, object data)
        {
            // Do nothing
        }

        public void Error(string message, Exception ex)
        {
            // Do nothing
        }

        /// <inheritdoc />
        public void Warning(string message, object data)
        {
            // Do nothing
        }

        public void Warning(string message)
        {
            // Do nothing
        }

        /// <inheritdoc />
        public void Info(string message, object data)
        {
            // Do nothing
        }

        public void Info(string message)
        {
            // Do nothing
        }

        /// <inheritdoc />
        public void Verbose(string message, object data)
        {
            // Do nothing
        }

        public void Verbose(string message)
        {
            // Do nothing
        }
    }
}