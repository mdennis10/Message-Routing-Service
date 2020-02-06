using System;
using EmailMessageRouter.Processor.Model;

namespace EmailMessageRouter.Processor.Messages
{
    public class ExecuteHandlersMsg : AbstractMsg
    {
        public ExecuteHandlersMsg(Guid requestId, Email email) : base(requestId)
        {
            Email = email;
        }
        public Email Email { get; }
    }
}