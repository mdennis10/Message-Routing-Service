using System;
using EmailMessageRouter.Data.EntityModel;

namespace EmailMessageRouter.Data.Repositories
{
    public interface IMessageRequestRepository : IRepository<Guid, MessageRequest>
    {
        
    }
}