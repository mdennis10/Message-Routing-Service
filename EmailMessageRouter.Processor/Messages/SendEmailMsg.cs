using System;
using EmailMessageRouter.Domain.Model;
using EmailMessageRouter.Processor.Model;

namespace EmailMessageRouter.Processor.Messages
{
    public class SendEmailMsg : AbstractMsg
    {
        public SendEmailMsg(
            Guid requestId, 
            Email email, 
            MessageType messageType
        ) : base(requestId)
        {
            Email = email;
            MessageType = messageType;
        }
        
        public MessageType MessageType { get; }
        public Email Email { get; }
    }
}