using System;

namespace EmailMessageRouter.Processor.Messages
{
    public abstract class AbstractMsg
    {
        public AbstractMsg(Guid requestId)
        {
            RequestId = requestId;
        }
        
        public Guid RequestId { get; }
    }
}