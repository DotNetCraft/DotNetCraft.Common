using System;
using DotNetCraft.Common.Core.DataAccessLayer;

namespace DotNetCraft.Common.DataAccessLayer
{
    public class DataContextPoolItem
    {
        public IDataContext DataContext { get; private set; }

        public int ReferenceCount { get; private set; }

        public DataContextPoolItem(IDataContext dataContext)
        {
            if (dataContext == null)
                throw new ArgumentNullException(nameof(dataContext));

            DataContext = dataContext;
            ReferenceCount = 1;
        }

        public int IncreseRef()
        {
            ReferenceCount++;
            return ReferenceCount;
        }

        public int DecreaseRef()
        {
            if (ReferenceCount <= 0)
                throw new IndexOutOfRangeException("ReferenceCount should be greater than 0.");

            ReferenceCount--;
            return ReferenceCount;
        }
    }
}
