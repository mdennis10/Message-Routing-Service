using System;
using EmailMessageRouter.Data.Cache;
using EmailMessageRouter.Data.EntityModel;
using EmailMessageRouter.Data.Repositories;
using EmailMessageRouter.Domain.Model;

namespace EmailMessageRouter.Domain.Services
{
    public class MessageRoutingServiceImpl : IMessageRoutingService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMessageRequestRepository _messageRequestRepository;
        private ICache<string, Account> _cache;

        public MessageRoutingServiceImpl(
            IAccountRepository accountRepository,
            IMessageRequestRepository messageRequestRepository,
            ICache<string, Account> cache)
        {
            _cache = cache;
            _accountRepository = accountRepository;
            _messageRequestRepository = messageRequestRepository;
        }

        public MessageType ResolveEmailMessageType(EmailMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException();
            }
            
            if (string.IsNullOrEmpty(message.From))
                return MessageType.Unknown;
            var account = _cache.Find(message.From);
            if ( account != null)
            {
                return resolveMessageType(account.SupportedMessageType);
            }
            account = _accountRepository.FindByEmail(message.From);
            if (account == null)
                return MessageType.Unknown;
            
            // Assuming the From emails don't change a cache
            // can be used to reduce the overhead on the
            // database read operations.
            _cache.Store(message.From, account);
            return resolveMessageType(account.SupportedMessageType);
        }
        
        private MessageType resolveMessgeType(int type) {
            switch(type){
                case 1:  return MessageType.Transactional;
                case 2:  return MessageType.NonTransactional:
                default: return MessageType.Unknown;
            }
        }

        public Guid StoreMessageRequest(Guid requestId, DateTime created, int total)
        {
            if (requestId == null) throw new ArgumentNullException(nameof(requestId));
            if (created == null)   throw new ArgumentException(nameof(created));
            return _messageRequestRepository.SaveOrUpdate(new MessageRequest
            {
                MessageRequestId = requestId,
                Created = created,
                TotalMessagesReceived = total
            });
        }
    }
}
