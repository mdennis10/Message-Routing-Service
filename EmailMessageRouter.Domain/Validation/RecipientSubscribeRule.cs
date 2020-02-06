using EmailMessageRouter.Domain.Model;

namespace EmailMessageRouter.Domain.Validation
{
    public class RecipientSubscribeRule : IEmailValidationRule
    {
        public ValidationResult Validate(EmailMessage entity)
        {
            throw new System.NotImplementedException();
        }
    }
}