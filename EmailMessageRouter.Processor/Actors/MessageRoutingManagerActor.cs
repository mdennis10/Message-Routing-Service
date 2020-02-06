using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;
using Akka.Persistence;
using AutoMapper;
using EmailMessageRouter.Data.Repositories;
using EmailMessageRouter.Domain.Model;
using EmailMessageRouter.Domain.Services;
using EmailMessageRouter.Processor.Messages;

namespace EmailMessageRouter.Processor.Actors
{
    /// <summary>
    /// The MessageRoutingManagerActor is a parent actor
    /// that orchestrate work to all child actors. 
    /// </summary>
    public class MessageRoutingManagerActor : BasePersistentActor
    {
        public override string PersistenceId => "EmailMessageRouter.Processor.Actors.MessageRoutingManagerActor";
        private readonly IEmailDeliveryService _emailDeliveryService;
        private readonly IMessageRoutingService _messageRoutingService;
        private readonly Mapper _mapper;
        private readonly IActorRef _messageResolverActor;
        private readonly IActorRef _messageValidationActor;
        private readonly IActorRef _messageSendingActor;
        private readonly IActorRef _messageHandlerExecutorActor;
        
        public MessageRoutingManagerActor(
            IMessageRoutingService messageRoutingService, 
            IEmailDeliveryService emailDeliveryService, 
            IAccountRepository accountRepository,
            Mapper mapper,
            int maxBatchSize,
            IDictionary<string, bool> validationRulesSettings,
            IDictionary<string, bool> handlersSettings)
        : base("MessageRoutingManagerActor")
        {
            _messageRoutingService = messageRoutingService;
            _emailDeliveryService = emailDeliveryService;
            
            // Automapper is used to convert data that needs to be transmitted
            // between logical application layers.
            _mapper = mapper;
            
            // create child actors
            _messageResolverActor = Context.ActorOf(
                MessageResolverActor.Props(messageRoutingService, mapper), 
                typeof(MessageResolverActor).Name
            );
            _messageValidationActor = Context.ActorOf(
                MessageValidationActor.Props(messageRoutingService, accountRepository, mapper, validationRulesSettings),
                typeof(MessageValidationActor).Name
            );
            _messageSendingActor = Context.ActorOf(
                MessageSendingActor.Props(emailDeliveryService, mapper, maxBatchSize),
                typeof(MessageSendingActor).Name
            );
            _messageHandlerExecutorActor = Context.ActorOf(
                MessageHandlerExecutorActor.Props(handlersSettings, mapper),
                typeof(MessageHandlerExecutorActor).Name
            );
            Command<EmailRequestMsg>(HandleEmailRequestMsg);
            Command<EmailResolvedMsg>(HandleEmailResolvedMsg);
            Command<EmailQualifyMsg>(HandleEmailQualifyMsg);
            Command<EmailDisqualifiedMsg>(HandleEmailDisqualifiedMsg);
        }

        public static Props Props(
            IMessageRoutingService messageRoutingService, 
            IEmailDeliveryService emailDeliveryService, 
            IAccountRepository accountRepository,
            Mapper mapper, 
            int maxBatchSize, 
            IDictionary<string, bool> validationRulesSettings,
            IDictionary<string, bool> handlersSettings)
        {
            return Akka.Actor.Props.Create(() => 
                new MessageRoutingManagerActor(
                    messageRoutingService, 
                    emailDeliveryService, 
                    accountRepository,
                    mapper, maxBatchSize, 
                    validationRulesSettings,
                    handlersSettings)
            );
        }
        
        /// <summary>
        /// Handle EmailRequestMsg received by sending a each email
        /// to MessageResolverActor for categorization.
        /// </summary>
        /// <param name="msg"></param>
        private void HandleEmailRequestMsg(EmailRequestMsg msg)
        {
            Persist(msg, emailRequestMsg =>
            {
                // stores information about message
                // request before starting processing
                _messageRoutingService.StoreMessageRequest(
                    emailRequestMsg.RequestId,
                    emailRequestMsg.Created,
                    emailRequestMsg.Emails.Count
                );
                emailRequestMsg.Emails.ForEach(email =>
                {
                    _messageResolverActor.Tell(new ResolveEmailTypeMsg(msg.RequestId, email), Self);
                });
            });
        }


        /// <summary>
        /// Instructs MessageValidationActor to validate email
        /// after it as been categorized.
        /// </summary>
        /// <param name="msg"></param>
        private void HandleEmailResolvedMsg(EmailResolvedMsg msg)
        {
            Persist(msg, emailResolvedMsg =>
            {
                var validateEmailMsg = new ValidateEmailMsg(
                    requestId: emailResolvedMsg.RequestId,
                    email: emailResolvedMsg.Email,
                    messageType: emailResolvedMsg.MessageType
                );
                _messageValidationActor.Tell(validateEmailMsg, Self);
            });
        }

        /// <summary>
        /// Sending email to MessageSendingActor which is sending
        /// email to correct downstream pipeline and also instructs
        /// MessageHandlerExecutorActor execute all configured handlers
        /// on email.
        /// after it as been categorized.
        /// </summary>
        /// <param name="msg"></param>
        private void HandleEmailQualifyMsg(EmailQualifyMsg msg)
        {
            Persist(msg, emailQualifyMsg =>
            {
                var sendEmailMsg = new SendEmailMsg(
                    requestId: emailQualifyMsg.RequestId, 
                    email: emailQualifyMsg.Email, 
                    messageType: emailQualifyMsg.MessageType
                );
                _messageSendingActor.Tell(sendEmailMsg, Self);

                var executeHandlersMsg = new ExecuteHandlersMsg(
                    requestId: emailQualifyMsg.RequestId,
                    email: emailQualifyMsg.Email
                );
                _messageHandlerExecutorActor.Tell(executeHandlersMsg, Self);
            });
            
        }

        /// <summary>
        /// Records disqualified emails.
        /// </summary>
        /// <param name="msg"></param>
        private void HandleEmailDisqualifiedMsg(EmailDisqualifiedMsg msg)
        {
            Persist(msg, emailDisqualifiedMsg =>
            {
                _emailDeliveryService.ProcessDisqualifiedEmail(
                    msg.RequestId, 
                    _mapper.Map<EmailMessage>(msg.Email), 
                    msg.MessageType
                );
            });
        }
    }
}