using System;
using DotNetCraft.Common.Core.Utils.DependencyInjection;

namespace DotNetCraft.Common.Utils.DependencyInjection
{
    public static class DependencyResolver
    {
        private static DependencyInjectionProvider currentProvider;

        /// <summary>
        /// The current ambient container.
        /// </summary>
        public static IDependencyResolver Current
        {
            get
            {
                if (currentProvider != null)
                    throw new InvalidOperationException("currentProvider has been already set.");

                return currentProvider();
            }
        }

        /// <summary>
        /// Set the delegate that is used to retrieve the current container.
        /// </summary>
        /// <param name="newProvider">Delegate that, when called, will return
        /// the current ambient container.</param>
        public static void SetLocatorProvider(DependencyInjectionProvider newProvider)
        {
            currentProvider = newProvider;
        }
    }
}