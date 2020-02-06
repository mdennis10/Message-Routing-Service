using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Persistence;
using AutoMapper;
using EmailMessageRouter.Domain.Handlers;
using EmailMessageRouter.Domain.Model;
using EmailMessageRouter.Processor.Messages;

namespace EmailMessageRouter.Processor.Actors
{
    /// <summary>
    /// MessageHandlerExecutorActor is used to process
    /// all enabled handlers. Execution of these handlers
    /// are assumed to be email qualification post-process.
    /// [Note] This can be refactored to evaluate the request in batches
    /// instead of individual emails.
    /// </summary>
    public class MessageHandlerExecutorActor : ReceivePersistentActor
    {
        public override string PersistenceId => "EmailMessageRouter.Processor.Actors.MessageHandlerExecutionActor";
        private readonly IEmailHandler _firstHandlers;
        private readonly Mapper _mapper;
        public MessageHandlerExecutorActor(Mapper mapper, IDictionary<string, bool> handlersSettings)
        {
            _mapper = mapper;
            _firstHandlers = ConfigureHandlerChain(handlersSettings);
            Command<ExecuteHandlersMsg>(HandleExecuteHandlersMsg);
        }

        public static Props Props(IDictionary<string, bool> configuredHandlers, Mapper mapper)
        {
            return Akka.Actor.Props.Create(() => new MessageHandlerExecutorActor(mapper, configuredHandlers));
        }

        /// <summary>
        /// Add enabled handlers to execution execution chain
        /// </summary>
        /// <param name="handlersSettings"></param>
        /// <returns></returns>
        private IEmailHandler ConfigureHandlerChain(IDictionary<string, bool> handlersSettings)
        {
            var handlers = new List<IEmailHandler>();
            bool enable;
            
            handlersSettings.TryGetValue(typeof(MessageReputationScoreHandler).FullName, out enable);
            if (enable)
            {
                handlers.Add(new MessageReputationScoreHandler());
            }
            handlersSettings.TryGetValue(typeof(SenderReputationScoreHandler).FullName, out enable);
            if (enable)
            {
                var nextHandler = new SenderReputationScoreHandler();
                handlers.Last().SetNextHandler(nextHandler);
                handlers.Add(nextHandler);
            }
            return handlers.First();
        }
        
        private void HandleExecuteHandlersMsg(ExecuteHandlersMsg msg)
        {
            Persist(msg, executeHandlersMsg =>
            {
                _firstHandlers.Process(_mapper.Map<EmailMessage>(msg.Email));
            });
        }
    }
}