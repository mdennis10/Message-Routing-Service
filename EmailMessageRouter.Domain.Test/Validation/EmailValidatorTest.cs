using System.Collections.Generic;
using EmailMessageRouter.Domain.Model;
using EmailMessageRouter.Domain.Validation;
using Moq;
using NUnit.Framework;

namespace EmailMessageRouter.Domain.Test.Validation
{
    public class EmailValidatorTest
    {
        [Test]
        public void Execute_AllValidationRuleReturnSuccessfulResultsTest()
        {
            var emailMessage = new EmailMessage
            {
                From = "johndoe@gmail.com",
                Subject = "Some Email Subject",
                To = "jane@gmail.com",
                HtmlBody = "<b>Hello</b>"
            };
            var mockValidationRule1 = new Mock<IEmailValidationRule>();
            var mockValidationRule2 = new Mock<IEmailValidationRule>();
            mockValidationRule1
                .Setup(x=> x.Validate(emailMessage))
                .Returns(ValidationResult.Success);
            mockValidationRule2
                .Setup(x => x.Validate(emailMessage))
                .Returns(ValidationResult.Success);
            var validationRules = new List<IEmailValidationRule>
            {
                mockValidationRule1.Object, 
                mockValidationRule2.Object
            };

            var emailValidator = new EmailValidator(validationRules);
            var results = emailValidator.Execute(emailMessage);
            
            Assert.NotNull(results);
            Assert.True(results.IsSuccess);
            Assert.True(results.FailureReasons.Length == 0);
        }
        
        [Test]
        public void Execute_NoValidationRuleIsPassedToValidatorTest() 
        {
            var emailMessage = new EmailMessage
            {
                From = "johndoe@gmail.com",
                Subject = "Some Email Subject",
                To = "jane@gmail.com",
                HtmlBody = "<b>Hello</b>"
            };
            
            var emailValidator = new EmailValidator(new List<IEmailValidationRule>());
            var results = emailValidator.Execute(emailMessage);
            
            Assert.NotNull(results);
            Assert.True(results.IsSuccess);
            Assert.True(results.FailureReasons.Length == 0);
        }

        [Test]
        public void Execute_AllValidationRuleReturnFailureResultsTest()
        {
            var emailMessage = new EmailMessage
            {
                From = "johndoe@gmail.com",
                Subject = "Some Email Subject",
                To = "jane@gmail.com",
                HtmlBody = "<b>Hello</b>"
            };
            var mockValidationRule1 = new Mock<IEmailValidationRule>();
            var mockValidationRule2 = new Mock<IEmailValidationRule>();
            mockValidationRule1
                .Setup(x=> x.Validate(emailMessage))
                .Returns(ValidationResult.Failed("To Email Address Invalid"));
            mockValidationRule2
                .Setup(x => x.Validate(emailMessage))
                .Returns(ValidationResult.Failed("From Email Address Invalid"));
            var validationRules = new List<IEmailValidationRule>
            {
                mockValidationRule1.Object, 
                mockValidationRule2.Object
            };

            var emailValidator = new EmailValidator(validationRules);
            var results = emailValidator.Execute(emailMessage);
            Assert.NotNull(results);
            Assert.False(results.IsSuccess);
            Assert.True(results.FailureReasons.Length == 2);
        }

        [Test]
        public void Execute_BothFailureAndSuccessValidationRuleReturnedTest()
        {
            var emailMessage = new EmailMessage
            {
                From = "johndoe@gmail.com",
                Subject = "Some Email Subject",
                To = "jane@gmail.com",
                HtmlBody = "<b>Hello</b>"
            };
            var mockValidationRule1 = new Mock<IEmailValidationRule>();
            var mockValidationRule2 = new Mock<IEmailValidationRule>();
            mockValidationRule1
                .Setup(x=> x.Validate(emailMessage))
                .Returns(ValidationResult.Success);
            mockValidationRule2
                .Setup(x => x.Validate(emailMessage))
                .Returns(ValidationResult.Failed("From Email Address Invalid"));
            var validationRules = new List<IEmailValidationRule>
            {
                mockValidationRule1.Object, 
                mockValidationRule2.Object
            };

            var emailValidator = new EmailValidator(validationRules);
            var results = emailValidator.Execute(emailMessage);
            Assert.NotNull(results);
            Assert.False(results.IsSuccess);
            Assert.True(results.FailureReasons.Length == 1);
        }
    }
}