using Akka.Actor;
using Akka.Event;
using Akka.Persistence;
using AutoMapper;
using EmailMessageRouter.Domain.Model;
using EmailMessageRouter.Domain.Services;
using EmailMessageRouter.Processor.Messages;

namespace EmailMessageRouter.Processor.Actors
{
    /// <summary>
    /// MessageResolverActor is responsible for resolving whether
    /// a email is Transactional or NonTransactional. This actor
    /// resolve each individual email because the Message Routing Service
    /// as no knowledge of the client providing emails and therefore cannot
    /// guarantee all messages contained within payload are transactional or not. 
    /// </summary>
    public class MessageResolverActor : ReceivePersistentActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();
        public override string PersistenceId => "EmailMessageRouter.Processor.Actors.MessageResolverActor";
        private readonly IMessageRoutingService _messageRoutingService;
        private readonly Mapper _mapper;
        
        public MessageResolverActor(IMessageRoutingService messageRoutingService, Mapper mapper)
        {
            _messageRoutingService = messageRoutingService;
            _mapper = mapper;
            Command<ResolveEmailTypeMsg>(HandleCategorizeEmailMsg);
        }

        public static Props Props(IMessageRoutingService messageRoutingService, Mapper mapper)
        {
            return Akka.Actor.Props.Create(() => new MessageResolverActor(messageRoutingService, mapper));
        }
        
        private void HandleCategorizeEmailMsg(ResolveEmailTypeMsg msg)
        {
            Persist(msg, x =>
            {
                var messageType = _messageRoutingService.ResolveEmailMessageType(_mapper.Map<EmailMessage>(msg));
                var emailResolvedMsg = new EmailResolvedMsg(
                    requestId: msg.RequestId, 
                    email: msg.Email, 
                    messageType: messageType
                );
                Context.Parent.Tell(emailResolvedMsg, Self);
            });
        }
    }
}