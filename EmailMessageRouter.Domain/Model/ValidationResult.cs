using System;

namespace EmailMessageRouter.Domain.Model
{
    public class ValidationResult
    {
        private ValidationResult(bool isSuccess, string msg)
        {
            IsSuccess = isSuccess;
            Message = msg;
        }
        public bool IsSuccess { get; }
        public string Message { get; }

        public static ValidationResult Failed(string failureMsg)
        {
            return new ValidationResult(false, failureMsg);
        }

        public static ValidationResult Success()
        {
            return new ValidationResult(true, String.Empty);
        }
    }
}