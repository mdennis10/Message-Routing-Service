using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Akka.Actor;
using AutoMapper;
using EmailMessageRouter.Processor.Messages;
using EmailMessageRouter.Processor.Model;
using EmailMessageRouter.Web.EntityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailMessageRouter.Web.Controllers
{
    /// <summary>
    /// Routing Service endpoint will be versioned to
    /// maintain graceful service evolution.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RoutingController : ControllerBase
    {
        private readonly IActorRef _messageRoutingManagerActor;
        private readonly Mapper _mapper;
        
        public RoutingController(IActorRef actor, Mapper mapper)
        {
            _messageRoutingManagerActor = actor;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult get()
        {
            return Ok("Welcome");
        }
        
        /// <summary>
        /// Given the high message volume this service will facilitate 
        /// a Fire Forget and non-blocking strategy  to provide
        /// high available and low latency.
        ///
        /// This api assumes both the single & bulk email API will handle validation
        /// of individual emails before they are sent. No additional validation will be done
        /// on submission of payload. If a required field is missing, a email address is invalid,
        /// or any other criteria that would invalidate a email from being sent, will be handling by
        /// the processing engine. This is to ensure entire batch is not failed by a few invalid emails.
        /// </summary>
        /// <param name="emailMessages"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<AcceptedResult> MessageRouter([FromBody] List<PostmarkEmail> emailMessages)
        {
            return Task.Run(() =>
            {
                var emails = _mapper.Map<List<PostmarkEmail>, List<Email>>(emailMessages);
                var msg = new EmailRequestMsg(emails);
                _messageRoutingManagerActor.Tell(msg, ActorRefs.NoSender);
                return Accepted(new PostmarkEmailResponse(msg.RequestId, emailMessages.Count));
            });
        }
    }
}
