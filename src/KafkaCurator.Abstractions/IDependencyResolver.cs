using System;
using System.Collections.Generic;

namespace KafkaCurator.Abstractions
{
    /// <summary>
    /// Dependency injection resolver wrapper
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Creates the scope lifetime of the dependency injection resolver
        /// </summary>
        /// <returns>The <see cref="IDependencyResolverScope"/></returns>
        IDependencyResolverScope CreateScope();
        
        /// <summary>
        /// Resolve an instance of the requested type
        /// </summary>
        /// <param name="type"><see cref="Type"/> of object to be resolved</param>
        /// <returns></returns>
        object Resolve(Type type);
        
        /// <summary>
        /// Resolve all instances configured for the given type
        /// </summary>
        /// <param name="type"><see cref="Type"/> of object to be resolved</param>
        /// <returns></returns>
        IEnumerable<object> ResolveAll(Type type);
        
        /// <summary>
        /// Resolve an instance of the relevant generic type
        /// </summary>
        /// <typeparam name="T"> of object to be resolved </typeparam>
        /// <returns></returns>
        T Resolve<T>();
        
        /// <summary>
        /// Resolve all instances configured for the relevant given generic type
        /// </summary>
        /// <typeparam name="T"> of objects to be resolved </typeparam>
        /// <returns></returns>
        IEnumerable<T> ResolveAll<T>();
    }
}