using System;
using EmailMessageRouter.Domain.Model;
using EmailMessageRouter.Processor.Model;

namespace EmailMessageRouter.Processor.Messages
{
    public class EmailDisqualifiedMsg : AbstractMsg
    {
        public EmailDisqualifiedMsg(
            Guid requestId, 
            Email email, 
            MessageType messageType,
            string[] reasons) : base(requestId)
        {
            Email = email;
            MessageType = messageType;
            Reasons = reasons;
        }
        
        public MessageType MessageType { get; }
        public Email Email { get; }
        public string[] Reasons { get; }
    }
}