using System;

namespace DotNetCraft.Common.Core.DataAccessLayer.DataContexts
{
    public interface IUniqueKey: IEquatable<IUniqueKey>
    {
        string Key { get; }
    }
}
