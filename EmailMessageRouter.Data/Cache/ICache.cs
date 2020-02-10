namespace EmailMessageRouter.Data.Cache
{
    /// <summary>
    /// This interface provide abstraction for
    /// underline cache system. For the purposes
    /// of this assessment a in-memory cache would
    /// be used. A Redis or MemCache implementation
    /// would be used in production.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="E"></typeparam>
    public interface ICache<K, E>
    {
        void Store(K key, E data);
        
        E Find(K key);
    }
}