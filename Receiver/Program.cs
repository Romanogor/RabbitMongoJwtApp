using Microsoft.Extensions.Configuration;
using RabbitMongoJwt.BL;
using RabbitMongoJwt.DAL;
using RabbitMongoJwt.DAL.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;

namespace Receiver
{
   public class Program
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public Program(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        public static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
               .AddJsonFile("appsettings.json", true, true)
               .Build();

            AppSettings appSettings = new AppSettings()
            {
                Databasename = config["Database:DatabaseName"],
                HostName = config["RabbitMq:HostName"],
                QueueName = config["RabbitMq:QueueName"]
            };

            ReceiveMessage(appSettings);
            
        }

        private static void ReceiveMessage(AppSettings appSettings)
        {
            UserRepository userRepository = new UserRepository(appSettings);
            UserService userService = new UserService(userRepository);

            var factory = new ConnectionFactory() { HostName = appSettings.HostName };

            using (var connection = factory.CreateConnection())

            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: appSettings.QueueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var userName = Encoding.UTF8.GetString(body);

                    userService.AddUser(new User { Name = userName, CreatedDate = DateTime.Now });

                };

                channel.BasicConsume(queue: appSettings.QueueName,
                                     autoAck: true,
                                     consumer: consumer);
                Console.ReadKey();
            }
        }
    }
}
