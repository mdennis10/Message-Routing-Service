using System;

namespace EmailMessageRouter.Data.EntityModel
{
    public class MessageRequest
    {
        public Guid MessageRequestId { get; set; }
        public DateTime Created { get; set; }
        public int TotalMessagesReceived { get; set; }
    }
}