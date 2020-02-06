using EmailMessageRouter.Data.EntityModel;

namespace EmailMessageRouter.Data.Repositories
{
    public class AccountRepositoryImpl : IAccountRepository
    {
        public Account Find(long key)
        {
            throw new System.NotImplementedException();
        }

        public long SaveOrUpdate(Account entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Account entity)
        {
            throw new System.NotImplementedException();
        }

        public Account FindByEmail(string email)
        {
            // this is just a mock therefore no implementation will be provided
            throw new System.NotImplementedException();
        }
    }
}