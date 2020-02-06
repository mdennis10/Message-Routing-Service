using System;
using EmailMessageRouter.Domain.Model;
using EmailMessageRouter.Processor.Model;

namespace EmailMessageRouter.Processor.Messages
{
    public class EmailQualifyMsg : AbstractMsg
    {
        public EmailQualifyMsg(
            Guid requestId, 
            Email email, 
            MessageType messageType) : base(requestId)
        {
            MessageType = messageType;
            Email = email;
        }
        
        public MessageType MessageType { get; }
        public Email Email { get; }
    }
}