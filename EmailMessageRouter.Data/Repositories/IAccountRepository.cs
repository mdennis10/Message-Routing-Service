using EmailMessageRouter.Data.EntityModel;

namespace EmailMessageRouter.Data.Repositories
{
    public interface IAccountRepository : IRepository<long, Account>
    {
        Account FindByEmail(string email);
    }
}