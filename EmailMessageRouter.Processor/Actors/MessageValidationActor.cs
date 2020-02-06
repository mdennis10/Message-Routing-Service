using System.Collections.Generic;
using Akka.Actor;
using Akka.Persistence;
using AutoMapper;
using EmailMessageRouter.Data.Repositories;
using EmailMessageRouter.Domain.Model;
using EmailMessageRouter.Domain.Services;
using EmailMessageRouter.Domain.Validation;
using EmailMessageRouter.Processor.Messages;

namespace EmailMessageRouter.Processor.Actors
{
    /// <summary>
    /// MessageValidationActor apply all configured validations
    /// to determine if a email should be sent or not. 
    /// </summary>
    public class MessageValidationActor : BasePersistentActor
    {
        public override string PersistenceId => "EmailMessageRouter.Processor.Actors.ValidationActor";
        private readonly IMessageRoutingService _messageRoutingService;
        private readonly EmailValidator _emailValidator;
        private readonly Mapper _mapper;
        private readonly IAccountRepository _accountRepository;
        public MessageValidationActor(
            IMessageRoutingService messageRoutingService, 
            IAccountRepository accountRepository, 
            Mapper mapper,
            IDictionary<string, bool> validationRulesSettings) 
        : base("MessageValidationActor")
        {
            _messageRoutingService = messageRoutingService;
            _accountRepository = accountRepository;
            _mapper = mapper;
            
            _emailValidator = new EmailValidator(ConfigureValidationRules(validationRulesSettings));
            Command<ValidateEmailMsg>(HandleValidateEmailMsg);
        }
        
        public static Props Props(
            IMessageRoutingService messageRoutingService, 
            IAccountRepository accountRepository,
            Mapper mapper, 
            IDictionary<string, bool> validationRules)
        {
            return Akka.Actor.Props.Create(() => new MessageValidationActor(
                messageRoutingService, accountRepository, mapper, validationRules)
            );
        }

        private List<IEmailValidationRule> ConfigureValidationRules(IDictionary<string, bool> validationRulesSettings)
        {
            var validationRules = new List<IEmailValidationRule>();
            bool enabled;
            validationRulesSettings.TryGetValue(typeof(SourceEmailValidationRule).FullName, out enabled);
            if(enabled) validationRules.Add(new SourceEmailValidationRule(_accountRepository));
            validationRulesSettings.TryGetValue(typeof(RecipientSubscribeRule).FullName, out enabled);
            if(enabled) validationRules.Add(new SourceEmailValidationRule(_accountRepository));
            return validationRules;
        }
        
        private void HandleValidateEmailMsg(ValidateEmailMsg msg)
        {
            Persist(msg, validateEmailMsg =>
            {
                var result = _emailValidator.Execute(_mapper.Map<EmailMessage>(msg.Email));
                if (result.IsSuccess)
                {
                    var emailQualifyMsg = new EmailQualifyMsg(
                        validateEmailMsg.RequestId, 
                        validateEmailMsg.Email, 
                        validateEmailMsg.MessageType
                    );
                    Context.Parent.Tell(emailQualifyMsg, Self);
                }
                else
                {
                    var emailQualifyMsg = new EmailDisqualifiedMsg(
                        validateEmailMsg.RequestId, 
                        validateEmailMsg.Email, 
                        validateEmailMsg.MessageType,
                        result.FailureReasons
                    );
                    Context.Parent.Tell(emailQualifyMsg, Self);
                }
            });
        }
    }
}