using System;

namespace KafkaCurator.Abstractions
{
    /// <summary>
    /// Represents the scope of a dependency injection resolver wrapper
    /// </summary>
    public interface IDependencyResolverScope : IDisposable
    {
        /// <summary>
        /// Gets the dependency injection resolver
        /// </summary>
        IDependencyResolver Resolver { get; }
    }
}