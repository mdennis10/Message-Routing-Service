using System;

namespace EmailMessageRouter.Data.Cache
{
    public class InMemoryCache<K, E> : ICache<K, E>
    {
        private readonly int cacheLifetime;

        public InMemoryCache(int cacheLifetime)
        {
            this.cacheLifetime = cacheLifetime;
        }

        public void Store(K key, E data)
        {
            // implementation goes here
        }

        public E Find(K key)
        {
            // implementation goes here
            return Activator.CreateInstance<E>(); 
        }
    }
}