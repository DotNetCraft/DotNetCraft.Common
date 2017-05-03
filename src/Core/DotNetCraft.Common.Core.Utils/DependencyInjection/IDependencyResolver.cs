using System;
using System.Collections.Generic;

namespace DotNetCraft.Common.Core.Utils.DependencyInjection
{
    /// <summary>
    /// This delegate is using for return the current container.
    /// </summary>
    /// <returns>The <see cref="IDependencyResolver"/> instance.</returns>
    public delegate IDependencyResolver DependencyInjectionProvider();

    /// <summary>
    /// Interface shows that object can resolve dependency injections
    /// </summary>
    public interface IDependencyResolver : IServiceProvider
    {
        /// <summary>
        /// Get an instance of the given serviceType.
        /// </summary>
        /// <param name="serviceType">The service's type.</param>
        /// <returns>The object if it has been created.</returns>
        object Resolve(Type serviceType);

        /// <summary>
        /// Get an instance of the given named  serviceType.
        /// </summary>
        /// <param name="serviceType">The service's type.</param>
        /// <param name="serviceName">The service's name that's been used for registration</param>
        /// <returns>The object if it has been created.</returns>
        object Resolve(Type serviceType, string serviceName);

        /// <summary>
        /// Get an instance of the given serviceType.
        /// </summary>
        /// <typeparam name="TService">The service's type.</typeparam>
        /// <returns>The object if it has been created.</returns>
        TService Resolve<TService>();

        /// <summary>
        /// Get an instance of the given named  serviceType.
        /// </summary>
        /// <typeparam name="TService">The service's type.</typeparam>
        /// <param name="serviceName">The service's name that's been used for registration</param>
        /// <returns>The object if it has been created.</returns>
        TService Resolve<TService>(string serviceName);

        /// <summary>
        /// Get list of instances of the given serviceType.
        /// </summary>
        /// <param name="serviceType">The service's type.</param>
        /// <returns>List of instances</returns>
        IEnumerable<object> ResolveAll(Type serviceType);

        /// <summary>
        /// Get list of instances of the given serviceType.
        /// </summary>
        /// <typeparam name="TService">The service's type.</typeparam>
        /// <returns>List of instances</returns>
        IEnumerable<TService> ResolveAll<TService>();
    }
}