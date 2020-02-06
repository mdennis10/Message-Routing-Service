using System;
using EmailMessageRouter.Domain.Model;

namespace EmailMessageRouter.Domain.Handlers
{
    public class SenderReputationScoreHandler : AbstractHandler<EmailMessage>, IEmailHandler
    {
        public override void Process(EmailMessage emailMessage)
        {
            if(emailMessage == null) throw new ArgumentNullException();
            
            // does some processing to evaluate message sender reputation score
            // CODE GOES HERE
            
            _nextHandler?.Process(emailMessage);
        }
    }
}