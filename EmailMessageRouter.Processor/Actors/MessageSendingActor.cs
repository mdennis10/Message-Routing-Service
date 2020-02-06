using System.Collections.Generic;
using Akka.Actor;
using Akka.Persistence;
using AutoMapper;
using EmailMessageRouter.Domain.Model;
using EmailMessageRouter.Domain.Services;
using EmailMessageRouter.Processor.Messages;
using EmailMessageRouter.Processor.Model;

namespace EmailMessageRouter.Processor.Actors
{
    public class MessageSendingActor : BasePersistentActor
    {
        public override string PersistenceId => "EmailMessageRouter.Processor.Actors.MessageSendingActor";
        private readonly IEmailDeliveryService _emailDeliveryService;
        private readonly Mapper _mapper;
        private readonly BulkEmailQueue _bulkEmailQueue;
        public MessageSendingActor(IEmailDeliveryService emailDeliveryService, Mapper mapper, int maxBatchSize)
        : base("MessageSendingActor")
        {
            _bulkEmailQueue = new BulkEmailQueue(Self, maxBatchSize);
            _emailDeliveryService = emailDeliveryService;
            _mapper = mapper;
            Command<SendEmailMsg>(HandleSendEmailMsg);
            Command<SendBatchEmailsMsg>(HandleSendBatchEmailsMsg);
        }

        public static Props Props(IEmailDeliveryService emailDeliveryService, Mapper mapper, int maxBatchSize)
        {
            return Akka.Actor.Props.Create(() => new MessageSendingActor(emailDeliveryService, mapper, maxBatchSize));
        }

        private void HandleSendEmailMsg(SendEmailMsg msg)
        {
            Persist(msg, sendEmailMsg =>
            {
                switch (msg.MessageType)
                {
                    case MessageType.Transactional: 
                        _emailDeliveryService.SendSingleEmail(_mapper.Map<EmailMessage>(msg.Email));
                        break;
                    case MessageType.NonTransactional:
                        _bulkEmailQueue.Enqueue(msg.Email);
                        break;
                }
            });
        }
        
        private void HandleSendBatchEmailsMsg(SendBatchEmailsMsg msg)
        {
            Persist(msg, sendBatchEmailsMsg =>
            {
                var emailMessages = _mapper.Map<List<Email>, List<EmailMessage>>(msg.Emails);
                _emailDeliveryService.SendingBatchEmail(emailMessages);
            });
        }
    }
}