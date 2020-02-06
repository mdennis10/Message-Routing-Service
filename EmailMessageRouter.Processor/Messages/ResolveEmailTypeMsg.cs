using System;
using EmailMessageRouter.Processor.Model;

namespace EmailMessageRouter.Processor.Messages
{
    public class ResolveEmailTypeMsg : AbstractMsg
    {
        public ResolveEmailTypeMsg(Guid requestId, Email email) : base(requestId)
        {
            Email = email;
        }
        
        public Email Email { get; }
    }
}