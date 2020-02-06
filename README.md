# Please see Message Routing Service.docx technical documentation at:
 
 [Message Routing Service.docx](https://github.com/mdennis10/Message-Routing-Service/blob/master/Message%20Routing%20Service.docx?raw=true)

## Requirements 
1. DotNET Core 3.1
2. Akka.NET
3. Consul Service Discovery
4. Fabio Load Balancer

## Deployment Instructions
### Build
Use the following dotnet command from project root folder to build application

**dotnet build**

### Test
Execute the following command at application root folder after a sucessful build

**dotnet test**

### Run
To run 
dotnet EmailMessageRouter.Web/bin/Debug/netcoreapp3.1/EmailMessageRouter.Web.dll

# REST EndPoint
api/v1/routing
```
[{
	"From": "sender@example.com", 
	"To": "receiver1@example.com", 
	"Subject": "Postmark test #1", 
	"HtmlBody": "<html><body><strong>Hello</strong> dear Postmark user.</body></html>"
},{
	"From": "sender@example.com", 
	"To": "receiver2@example.com", 
	"Subject": "Postmark test #2", 
	"HtmlBody": "<html><body><strong>Hello</strong> dear Postmark user.</body></html>"
}]
```

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




