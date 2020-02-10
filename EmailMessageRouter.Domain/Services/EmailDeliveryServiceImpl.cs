using System;
using System.Collections.Generic;
using System.Text.Json;
using EmailMessageRouter.Domain.Model;

namespace EmailMessageRouter.Domain.Services
{
    public class EmailDeliveryServiceImpl : IEmailDeliveryService
    {
        
        public void SendSingleEmail(EmailMessage emailMessage)
        {
            // Send Email to transactional email downstream REST API.
            Console.WriteLine(JsonSerializer.Serialize(emailMessage));
        }

        public void SendingBatchEmail(List<EmailMessage> emailMessage)
        {
            // This will send email batch to the bulk email downstream REST API.
            // The downstream API could also be Message Queue, however a REST API would 
            // provide a standard interface for communication and interoperability. 
            Console.WriteLine(JsonSerializer.Serialize(emailMessage));
        }

        public void ProcessDisqualifiedEmail(Guid requestId, EmailMessage email, MessageType messageType)
        {
            // This will store disqualified emails
        }
    }
}