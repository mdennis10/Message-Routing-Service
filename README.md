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
Run the following command at root folder after a sucessful build to run automated tests.

**dotnet test**

### Run
To run application 

**dotnet EmailMessageRouter.Web/bin/Debug/netcoreapp3.1/EmailMessageRouter.Web.dll**

# REST EndPoint
POST api/v1/routing

**Request Body**
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

**Response Body**
```
{
    "requestId": "618da6f6-dcc0-4221-b032-b5047a5f1e8b",
    "created": "2020-02-05T22:41:10.1086697-05:00",
    "total": 2
}
```

submit a POST request to endpoint then check application console log to test application behavior.

## Application Config

Application configuration can be found in the **appsettings.json** EmailMessageRouter.Web folder.

**MaxBatchEmails** - sets queue size for emails that are sent to bulk email downstream pipeline.

```
 "MaxBatchEmails": 50,
 ```

**CacheLifetime** - set the lifespan a item is stored in cache, which is represented in minutes.

```
"CacheLifetime":  10,
```

**ValidationRules** - configure all validation rules that should be executed by Message Processor. Only validation rules that are defined and set to true will be executed.

```
"ValidationRules": {
    "EmailMessageRouter.Domain.Validation.RecipientSubscribeRule": true,
    "EmailMessageRouter.Domain.Validation.SourceEmailValidationRule": true
  }
```

**BusinessEvaluationHandlers** - configure all handlers that should be executed by Message Processor. Only handlers that are defined and set to true will be executed.

```
"BusinessEvaluationHandlers":{
    "EmailMessageRouter.Domain.Handlers.MessageReputationScoreHandler":true,
    "EmailMessageRouter.Domain.Handlers.SenderReputationScoreHandler": true
  }
```

## Akka.NET Info
Actor class are not instantiated directly but instead by passing a 
Prop to actor system which creates and manages the lifecycle of actor processes.

**Parent & Child Actors**

All actors created by a actor are children of that actor. The parent actor
supervises the children actors and tell them what do when they encounter failures.
For the purpose of this assessment the default behavior will be used which is to
instruct child actors to restart.

**Command method**

The actor Command is used to configure which actor method
should handle a given messsage.

**Persist method**

The actor Persist method is used to save messages to actor system
journal. This messages are replayed to for actors to recover where 
they stopped. For the purpose of the assessment a in-memory journal is
used however mongoDB would be used in a production situation.

**IActorRef class**

IActorRef is used to send message to actors. It is return by the actor system when
when actors are created.




