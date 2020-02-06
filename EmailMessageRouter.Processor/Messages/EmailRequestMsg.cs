using System;
using System.Collections.Generic;
using EmailMessageRouter.Processor.Model;

namespace EmailMessageRouter.Processor.Messages
{
    public class EmailRequestMsg : AbstractMsg
    {
        public EmailRequestMsg(List<Email> emails) : base (Guid.NewGuid())
        {
            Created = DateTime.Now;
            Emails = emails;
        }
        
        public DateTime Created { get; }
        public List<Email> Emails { get; }
    }
}