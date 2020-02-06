using EmailMessageRouter.Data.Cache;
using EmailMessageRouter.Data.EntityModel;
using EmailMessageRouter.Data.Repositories;
using EmailMessageRouter.Domain.Model;
using EmailMessageRouter.Domain.Services;
using Moq;
using NUnit.Framework;

namespace EmailMessageRouter.Domain.Test.Services
{
    public class MessageRoutingServiceTest
    {
        [Test]
        public void ResolveEmailMessageType_NullOrEmptyEmailFromPropertySupplied()
        {
            var accountRepo = new Mock<IAccountRepository>();
            var messageRequestRepo = new Mock<IMessageRequestRepository>();
            var cache = new Mock<ICache<string, Account>>();
            var service = new MessageRoutingServiceImpl(
                accountRepo.Object,
                messageRequestRepo.Object,
                cache.Object
            );
            var message = new EmailMessage
            {
                From = "johndoe@gmail.com",
                Subject = "Test Subject",
                To = "janedoe@gmail.com",
                HtmlBody = "some html content"
            };
            var account = new Account
            {
                AccountId = 1,
                Email = message.From,
                IsActive = true,
                SupportedMessageType = 1
            };
            accountRepo
                .Setup(x => x.FindByEmail(message.From))
                .Returns(account);
            cache
                .Setup(x => x.Find(message.From))
                .Returns(account);
            message.From = null;
            var messageType = service.ResolveEmailMessageType(message);
            Assert.AreEqual(MessageType.Unknown, messageType);
        }

        [Test]
        public void ResolveEmailMessageType()
        {
            var accountRepo = new Mock<IAccountRepository>();
            var messageRequestRepo = new Mock<IMessageRequestRepository>();
            var cache = new Mock<ICache<string, Account>>();
            var service = new MessageRoutingServiceImpl(
                accountRepo.Object,
                messageRequestRepo.Object,
                cache.Object
            );
            var message = new EmailMessage
            {
                From = "johndoe@gmail.com",
                Subject = "Test Subject",
                To = "janedoe@gmail.com",
                HtmlBody = "some html content"
            };
            var account = new Account
            {
                AccountId = 1,
                Email = message.From,
                IsActive = true,
                SupportedMessageType = 1
            };
            accountRepo
                .Setup(x => x.FindByEmail(message.From))
                .Returns(account);
            cache
                .Setup(x => x.Find(message.From))
                .Returns(account);
            var messageType = service.ResolveEmailMessageType(message);
            Assert.AreEqual(MessageType.Transactional, messageType);
        }
    }
}