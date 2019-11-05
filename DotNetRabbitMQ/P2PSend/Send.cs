using RabbitMQ.Client;
using System;
using System.Text;

namespace P2PSend
{
    class Send
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "49.232.164.202",
                UserName = "admin",
                Password = "admin",
                Port = AmqpTcpEndpoint.UseDefaultPort,
                VirtualHost = "/",
                Protocol = Protocols.DefaultProtocol
            };
            using (var connection = factory.CreateConnection())
            
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello", 
                                                       durable: false, 
                                                       exclusive: false,
                                                       autoDelete: false, 
                                                       arguments: null);

                    string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                                                    routingKey: "hello",
                                                    basicProperties: null,
                                                    body: body);
                    Console.WriteLine("[x] sent {0}", message);
                }
            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();
        }
    }
}
