using System;
using EmailMessageRouter.Domain.Model;

namespace EmailMessageRouter.Domain.Handlers
{
    public class MessageReputationScoreHandler : AbstractHandler<EmailMessage>, IEmailHandler
    {
        public override void Process(EmailMessage emailMessage)
        {
            if(emailMessage == null) throw new ArgumentNullException();
            
            // Does some processing to evaluate message sender reputation score
            // [NOTE] These handlers would read any configuration needed for
            // evaluation from database. This provide the ability to dynamically
            // change parameters used for evaluation without application restart.
            
            // TODO CODE GOES HERE
            
            _nextHandler?.Process(emailMessage);
        }
    }
}