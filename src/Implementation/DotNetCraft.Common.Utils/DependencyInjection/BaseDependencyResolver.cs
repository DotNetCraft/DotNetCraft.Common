using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.Utils.DependencyInjection;
using DotNetCraft.Common.Utils.DependencyInjection.Exceptions;

namespace DotNetCraft.Common.Utils.DependencyInjection
{
    /// <summary>
    /// Base class for all dependency resolvers.
    /// </summary>
    public abstract class BaseDependencyResolver: IDependencyResolver
    {
        #region Resolve single instance...

        /// <summary>
        /// Occurs when single instance should be resolved and returned.
        /// </summary>
        /// <param name="serviceType">The service's type.</param>
        /// <param name="serviceName">The service's name that's been used for registration</param>
        /// <returns>The object if it has been created.</returns>
        protected abstract object OnResolve(Type serviceType, string serviceName);

        /// <summary>
        /// Get an instance of the given serviceType.
        /// </summary>
        /// <param name="serviceType">The service's type.</param>
        /// <returns>The object if it has been created.</returns>
        public virtual object Resolve(Type serviceType)
        {
            return Resolve(serviceType, null);
        }

        /// <summary>
        /// Get an instance of the given named  serviceType.
        /// </summary>
        /// <param name="serviceType">The service's type.</param>
        /// <param name="serviceName">The service's name that's been used for registration</param>
        /// <returns>The object if it has been created.</returns>
        public virtual object Resolve(Type serviceType, string serviceName)
        {
            try
            {
                return OnResolve(serviceType, serviceName);
            }
            catch (Exception ex)
            {
                throw new ResolveException("There was a problem during activation an object", ex, new Dictionary<string, string>
                {
                    {"serviceType", serviceType?.ToString()},
                    {"serviceName", serviceName}
                });
            }
        }        

        /// <summary>
        /// Get an instance of the given serviceType.
        /// </summary>
        /// <typeparam name="TService">The service's type.</typeparam>
        /// <returns>The object if it has been created.</returns>
        public virtual TResolver Resolve<TResolver>()
        {
            return (TResolver)Resolve(typeof(TResolver), null);
        }

        /// <summary>
        /// Get an instance of the given named  serviceType.
        /// </summary>
        /// <typeparam name="TService">The service's type.</typeparam>
        /// <param name="serviceName">The service's name that's been used for registration</param>
        /// <returns>The object if it has been created.</returns>
        public virtual TResolver Resolve<TResolver>(string serviceName)
        {
            return (TResolver)Resolve(typeof(TResolver), serviceName);
        }

        #endregion

        #region Resolve many...

        /// <summary>
        /// Occurs when serviceType should be resolved and all instances should be returned. 
        /// </summary>
        /// <param name="serviceType">The service's type.</param>
        /// <returns>List of instances</returns>
        protected abstract IEnumerable<object> OnResolveAll(Type serviceType);

        /// <summary>
        /// Get list of instances of the given serviceType.
        /// </summary>
        /// <param name="serviceType">The service's type.</param>
        /// <returns>List of instances</returns>
        public virtual IEnumerable<object> ResolveAll(Type serviceType)
        {
            try
            {
                return OnResolveAll(serviceType);
            }
            catch (Exception ex)
            {
                throw new ResolveException("There was a problem during activation list of objects", ex, new Dictionary<string, string>
                {
                    {"serviceType", serviceType?.ToString()}
                });
            }
        }

        /// <summary>
        /// Get list of instances of the given serviceType.
        /// </summary>
        /// <typeparam name="TService">The service's type.</typeparam>
        /// <returns>List of instances</returns>
        public virtual IEnumerable<TService> ResolveAll<TService>()
        {
            foreach (object item in ResolveAll(typeof(TService)))
            {
                yield return (TService)item;
            }
        }

        #endregion


        #region Implementation of IServiceProvider

        /// <summary>Gets the service object of the specified type.</summary>
        /// <returns>A service object of type <paramref name="serviceType" />.-or- null if there is no service object of type <paramref name="serviceType" />.</returns>
        /// <param name="serviceType">An object that specifies the type of service object to get. </param>
        public object GetService(Type serviceType)
        {
            return Resolve(serviceType);
        }

        #endregion
    }
}