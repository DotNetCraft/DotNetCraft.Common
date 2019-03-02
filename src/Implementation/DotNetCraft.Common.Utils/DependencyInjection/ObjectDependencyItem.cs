using System;
using DotNetCraft.Common.Core.Utils.DependencyInjection;

namespace DotNetCraft.Common.Utils.DependencyInjection
{
    public class ObjectDependencyItem<TObject> : IDependencyItem
    {
        private readonly TObject _instance;

        public ObjectDependencyItem(TObject instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            _instance = instance;
        }

        #region Implementation of IDependencyItem

        public object GetOrCreateObject(params object[] args)
        {
            if (args != null && args.Length != 0)
                throw new ArgumentOutOfRangeException("args", "No arguments allowed. Args count: " + args.Length);

            return _instance;
        }

        #endregion
    }
}
