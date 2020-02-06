using System;
using EmailMessageRouter.Data.Repositories;
using EmailMessageRouter.Domain.Model;

namespace EmailMessageRouter.Domain.Validation
{
    public class SourceEmailValidationRule : IEmailValidationRule
    {
        private readonly IAccountRepository _accountRepository;

        public SourceEmailValidationRule(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public ValidationResult Validate(EmailMessage entity)
        {
            if(entity == null) throw new ArgumentNullException();
            if (string.IsNullOrEmpty(entity.From))
            {
                return ValidationResult.Failed("No From email address present");
            }
            var account = _accountRepository.FindByEmail(entity.From);
            if (account == null)
            {
                return ValidationResult.Failed("Account not found");
            }
            return (!account.IsActive) ? 
                ValidationResult.Failed("Account is not active") :
                ValidationResult.Success();
        }
    }
}