using System.Collections;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace Pamux.Lib.Utilities
{
    public class LruCache<K, V>
    {
        private int capacity;
        private IDictionary<K, LinkedListNode<LruCacheItem<K, V>>> cacheMap = new Dictionary<K, LinkedListNode<LruCacheItem<K, V>>>();
        private LinkedList<LruCacheItem<K, V>> lruList = new LinkedList<LruCacheItem<K, V>>();

        public LruCache(int capacity)
        {
            this.capacity = capacity;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public V Get(K key)
        {
            LinkedListNode<LruCacheItem<K, V>> node;
            if (cacheMap.TryGetValue(key, out node))
            {
                var value = node.Value.value;
                lruList.Remove(node);
                lruList.AddLast(node);
                return value;
            }
            return default(V);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Set(K key, V val)
        {
            if (cacheMap.Count >= capacity)
            {
                RemoveFirst();
            }

            var cacheItem = new LruCacheItem<K, V>(key, val);
            var node = new LinkedListNode<LruCacheItem<K, V>>(cacheItem);
            lruList.AddLast(node);
            cacheMap.Add(key, node);
        }

        private void RemoveFirst()
        {
            var node = lruList.First;
            lruList.RemoveFirst();

            cacheMap.Remove(node.Value.key);
        }
    }

    class LruCacheItem<K, V>
    {
        public LruCacheItem(K k, V v)
        {
            key = k;
            value = v;
        }

        public K key;
        public V value;
    }
}