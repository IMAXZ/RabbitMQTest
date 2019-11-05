using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace P2PReceive
{
    class Receive
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

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("[x] Received {0}", message);
                };
                channel.BasicConsume(queue: "hello",
                                                    autoAck: true,
                                                    consumer: consumer);
                Console.WriteLine("Press [Enter] to exit.");
                Console.ReadLine();
            }   
        }
    }
}
