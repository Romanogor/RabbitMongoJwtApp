using Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMongoJwt.BL
{
    public class MessageService : IMessageService
    {
        private readonly IAppSettings _appSettings;

        public MessageService(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void SendMessageToQueue(string userName)
        {
            var factory = new ConnectionFactory() { HostName = _appSettings.HostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _appSettings.QueueName,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);



                    var body = Encoding.UTF8.GetBytes(userName);

                    channel.BasicPublish(exchange: "",
                                         routingKey: _appSettings.QueueName,
                                         basicProperties: null,
                                         body: body);

                }
            }
        }


    }
}
