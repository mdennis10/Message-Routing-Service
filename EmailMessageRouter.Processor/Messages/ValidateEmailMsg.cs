using System;
using EmailMessageRouter.Domain.Model;
using EmailMessageRouter.Processor.Model;

namespace EmailMessageRouter.Processor.Messages
{
    public class ValidateEmailMsg : AbstractMsg
    {
        public ValidateEmailMsg(
            Guid requestId, 
            Email email, 
            MessageType messageType) : base(requestId)
        {
            Email = email;
            MessageType = messageType;
        }
        
        public MessageType MessageType { get; }
        public Email Email { get; }
    }
}