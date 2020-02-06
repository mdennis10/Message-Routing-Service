using EmailMessageRouter.Domain.Model;

namespace EmailMessageRouter.Domain.Validation
{
    public interface IEmailValidationRule : IValidationRule<EmailMessage>
    {
        
    }
}