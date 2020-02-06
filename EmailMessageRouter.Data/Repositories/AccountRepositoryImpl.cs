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
            // mock implementation
            return 1;
        }

        public void Delete(Account entity)
        {
            // mock implementation
        }

        public Account FindByEmail(string email)
        {
            // this is just a mock therefore no implementation will be provided
            return new Account
            {
                Email = email,
                IsActive = true,
                AccountId = 1,
                SupportedMessageType = 1
            };
        }
    }
}