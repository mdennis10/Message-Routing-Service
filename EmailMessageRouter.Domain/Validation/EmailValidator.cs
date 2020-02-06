using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EmailMessageRouter.Domain.Model;

namespace EmailMessageRouter.Domain.Validation
{
    public class EmailValidator
    {
        private readonly List<IEmailValidationRule> _validationRules;
        public EmailValidator(List<IEmailValidationRule> validationRules)
        {
            _validationRules = validationRules;
        }
        
        public ValidationResults Execute(EmailMessage entity)
        {
            var results = new List<ValidationResult>();
            _validationRules.ForEach(rule => results.Add(rule.Validate(entity)));
            var status = results.All(x => x.IsSuccess);
            if (status)
            {
                return new ValidationResults(status, new string[] { });
            }
            // remove all elements that are not failures
            results.RemoveAll(x => x.IsSuccess);
            return new ValidationResults(status, results.Select(x => x.Message).ToArray());
        }
    }
}