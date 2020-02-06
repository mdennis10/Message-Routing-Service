using EmailMessageRouter.Domain.Handlers;
using EmailMessageRouter.Domain.Model;
using Moq;
using NUnit.Framework;

namespace EmailMessageRouter.Domain.Test.Handlers
{
    public class MessageReputationScoreHandlerTest
    {
        [Test]
        public void Process_InvokeNextHandlerInChainSuccessfullyTest()
        {
            var firstHandler = new MessageReputationScoreHandler();
            var nextHandler= new Mock<MessageReputationScoreHandler>();
            firstHandler.SetNextHandler(nextHandler.Object);
            var message = new EmailMessage
            {
                From = "johndoe@gmail.com",
                Subject = "Test Subject",
                To = "janedoe@gmail.com"
            };
            firstHandler.Process(message);
            nextHandler.Verify(x => x.Process(message),Times.Once());
        }
    }
}