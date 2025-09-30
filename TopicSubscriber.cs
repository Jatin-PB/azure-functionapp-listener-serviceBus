using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ServiceBusListener2.Data;
using ServiceBusListener2.Model;
using System;
using System.Threading.Tasks;


namespace ServiceBusListener2
{



    public class TopicSubscriber
    {
        private readonly MessagesDbContext _db;
        private readonly ILogger<TopicSubscriber> _logger;

        public TopicSubscriber(MessagesDbContext db, ILogger<TopicSubscriber> logger)
        {
            _db = db;
            _logger = logger;
        }

        [Function(nameof(TopicSubscriber))]
        public async Task Run(
            [ServiceBusTrigger("poc-messages", "kcwSubscriber", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {

            var connStr = Environment.GetEnvironmentVariable("SqlConnection");

            if (string.IsNullOrWhiteSpace(connStr))
            {
                throw new InvalidOperationException("Environment variable 'SqlConnection' is not set.");
            }
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            var entity = new ServiceBusMessageEntity
            {
                MessageBody = message.Body.ToString(),
                ReceivedAt = DateTime.UtcNow
            };

            _db.Messages.Add(entity);
            await _db.SaveChangesAsync();

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }


    //public class TopicSubscriber
    //{
    //    private readonly ILogger<TopicSubscriber> _logger;

    //    public TopicSubscriber(ILogger<TopicSubscriber> logger)
    //    {
    //        _logger = logger;
    //    }

    //    [Function(nameof(TopicSubscriber))]
    //    public async Task Run(
    //        [ServiceBusTrigger("poc-messages", "kcwSubscriber", Connection = "ServiceBusConnection")]
    //        ServiceBusReceivedMessage message,
    //        ServiceBusMessageActions messageActions)
    //    {

    //        var connStr = Environment.GetEnvironmentVariable("SqlConnection");

    //        //            if (string.IsNullOrWhiteSpace(connStr))
    //        //            {
    //        //                throw new InvalidOperationException("Environment variable 'SqlConnection' is not set.");
    //        //            }
    //        _logger.LogInformation("Message ID: {id}", message.MessageId);
    //        _logger.LogInformation("Message Body: {body}", message.Body);
    //        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

    //        // Complete the message
    //        await messageActions.CompleteMessageAsync(message);
    //    }
    //}


}
