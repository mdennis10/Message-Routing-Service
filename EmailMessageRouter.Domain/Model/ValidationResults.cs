using System.Collections.Generic;

namespace EmailMessageRouter.Domain.Model
{
    public class ValidationResults
    {
        public ValidationResults(bool status, string[] failureReasons)
        {
            IsSuccess = status;
            FailureReasons = failureReasons;
        }
        
        public bool IsSuccess { get; }
        public string[] FailureReasons { get; }
    }
}