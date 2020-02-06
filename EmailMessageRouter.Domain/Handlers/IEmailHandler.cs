using EmailMessageRouter.Domain.Model;

namespace EmailMessageRouter.Domain.Handlers
{
    public interface IEmailHandler : IHandler<EmailMessage>
    {
        
    }
}