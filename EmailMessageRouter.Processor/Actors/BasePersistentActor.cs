using System;
using System.Text.Json;
using Akka.Event;
using Akka.Persistence;

namespace EmailMessageRouter.Processor.Actors
{
    public abstract class BasePersistentActor : ReceivePersistentActor
    {
        private readonly string _actorName;
        private readonly ILoggingAdapter _log = Context.GetLogger();
        public BasePersistentActor(string actorName)
        {
            _actorName = actorName;
        }

        protected ILoggingAdapter GetLogger() => _log;
        protected override void PreStart()
        {
            base.PreStart();
            GetLogger().Info($"{_actorName} started");
        }
        
        protected override bool Receive(object message)
        {
            var messageType = message.GetType().Name;
            GetLogger().Info($"{_actorName} received: {messageType}");
            return base.Receive(message);
        }

        protected override void PostRestart(Exception reason)
        {
            base.PostRestart(reason);
            GetLogger().Info($"{_actorName} restarted");
        }

        protected override void PostStop()
        {
            base.PostStop();
            GetLogger().Info($"{_actorName} stopped");
        }
    }
}