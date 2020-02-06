using System;
using System.Collections.Generic;
using EmailMessageRouter.Processor.Model;

namespace EmailMessageRouter.Processor.Messages
{
    public class SendBatchEmailsMsg
    {
        public SendBatchEmailsMsg(List<Email> emails)
        {
            Emails = emails;
        }
        
        public List<Email> Emails { get; }
    }
}