using System;
using EmailMessageRouter.Data.EntityModel;

namespace EmailMessageRouter.Data.Repositories
{
    public class MessageRequestRepositoryImpl : IMessageRequestRepository
    {
        public MessageRequest Find(Guid key)
        {
            return new MessageRequest();
        }

        public Guid SaveOrUpdate(MessageRequest entity)
        {
            // this just mocks save or update operation
            return Guid.NewGuid();
        }

        public void Delete(MessageRequest entity)
        {
            // implementation does here
        }
    }
}