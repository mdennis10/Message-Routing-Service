namespace EmailMessageRouter.Data.Repositories
{
    public interface IRepository<K, E>
    {
        E Find(K key);
        K SaveOrUpdate(E entity);
        void Delete(E entity);
    }
}