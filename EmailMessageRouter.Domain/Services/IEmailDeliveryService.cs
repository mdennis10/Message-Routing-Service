using System;
using System.Collections.Generic;
using EmailMessageRouter.Domain.Model;

namespace EmailMessageRouter.Domain.Services
{
    public interface IEmailDeliveryService
    {
        void SendSingleEmail(EmailMessage emailMessage);
        void SendingBatchEmail(List<EmailMessage> emailMessage);

        /// <summary>
        /// Should the business decide this information is just transaction this method
        /// will store data to mongodb. however if this information is required for further processing
        /// such as generating reports, additional endpoints for client to check status of request and
        /// a explicit data scheme can be defined via data model then a rational database can be used.
        /// This will be a write intensive operation which should be considered when choosing underline data store.
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="email"></param>
        /// <param name="messageType"></param>
        void ProcessDisqualifiedEmail(Guid requestId, EmailMessage email, MessageType messageType);
    }
}