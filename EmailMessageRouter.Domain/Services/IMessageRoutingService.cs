using System;
using EmailMessageRouter.Domain.Model;

namespace EmailMessageRouter.Domain.Services
{
    public interface IMessageRoutingService
    {
        /// <summary>
        /// Inspects email to find out whether it a transactional message are not.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        MessageType ResolveEmailMessageType(EmailMessage message);
        Guid StoreMessageRequest(Guid requestId, DateTime created, int total);
    }
}