using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using EmailMessageRouter.Processor.Messages;

namespace EmailMessageRouter.Processor.Model
{
    public class BulkEmailQueue
    {
        private Queue<Email> _queue;
        private readonly IActorRef _actorRef;
        private readonly int _maxBatchSize;
        public BulkEmailQueue(IActorRef actorRef, int maxBatchSize)
        {
            _actorRef = actorRef;
            _maxBatchSize = maxBatchSize;
            _queue = new Queue<Email>();
        }

        public void Enqueue(Email email)
        {
            _queue.Enqueue(email);
            lock (_queue)
            {
                if (_queue.Count != _maxBatchSize) return;
                var emails = _queue.ToList();
                _queue.Clear();
                _actorRef.Tell(new SendBatchEmailsMsg(emails), ActorRefs.NoSender);
            }
        }
    }
}