using EmailMessageRouter.Domain.Model;

namespace EmailMessageRouter.Domain.Validation
{
    public interface IValidationRule <in T>
    {
        ValidationResult Validate(T entity);
    }
}