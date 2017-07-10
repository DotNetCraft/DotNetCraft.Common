using System;
using System.Collections.Generic;
using DotNetCraft.Common.Core.Utils.Caching;

namespace DotNetCraft.Common.Utils.Caching
{
    public class SimpleCache<TKey, TValue>: ISimpleCache<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> dictionary;
        private readonly object syncObject = new object();

        public SimpleCache()
        {
            dictionary = new Dictionary<TKey, TValue>();
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            lock (syncObject)
            {
                if (dictionary.ContainsKey(key))
                    throw new ArgumentException("An element with the same key already exists in the dictionary.", "key");

                dictionary.Add(key, value);
            }                           
        }

        public void Update(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            lock (syncObject)
            {
                if (dictionary.ContainsKey(key) == false)
                    throw new ArgumentException("There is no element with the key in the dictionary.", "key");

                dictionary[key] = value;
            }
        }

        public bool Delete(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            lock (syncObject)
            {
                bool result = dictionary.Remove(key);
                return result;
            }
        }

        public void AddOrUpdate(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            lock (syncObject)
            {
                TValue existingItem;
                if (dictionary.TryGetValue(key, out existingItem))
                    dictionary[key] = value;
                else
                    dictionary.Add(key, value);
            }
        }

        public TValue Get(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            lock (syncObject)
            {
                TValue existingItem;
                if (dictionary.TryGetValue(key, out existingItem))
                    return existingItem;
            }

            throw new KeyNotFoundException("The key does not exist in the collection.");
        }

        public bool TryGetValue(TKey key, out TValue result)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            
            lock (syncObject)
            {
                TValue existingItem;
                if (dictionary.TryGetValue(key, out existingItem))
                {
                    result = existingItem;
                    return true;
                }
            }

            result = default(TValue);
            return false;
        }
    }
}
