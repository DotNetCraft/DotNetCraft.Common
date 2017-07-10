namespace DotNetCraft.Common.Core.Utils.Caching
{
    public interface ISimpleCache<TKey, TValue>
    {
        void Add(TKey key, TValue value);

        void Update(TKey key, TValue value);

        bool Delete(TKey key);

        void AddOrUpdate(TKey key, TValue value);

        TValue Get(TKey key);

        bool TryGetValue(TKey key, out TValue result);        
    }
}
