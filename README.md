# Please see Message Routing Service.docx for technical documentation



# Akka.NET Info
Actor class are not instantiated directly but instead by passing a 
Prop to actor system which creates and manages the lifecycle of actor processes.

## Parent & Child Actors
All actors created by a actor are children of that actor. The parent actor
supervises the children actor and tell them what do when they encounter failures.
For the purpose of this assessment the default behavior will be used which is to
instruct child actors to restart.

## Command method
The actor Command is used to tell which actor method
should process a give messsage type

## Persist method
The actor Persist method is used to save messages to actor system
journal. This messages are replayed to for actors to recover where 
they stopped. For the purpose of the assessment a in-memory journal is
used however mongoDB would be used in a production situation.

## IActorRef class
IActorRef is used to send message to actors. It is return by the actor system when
when actors are created.




