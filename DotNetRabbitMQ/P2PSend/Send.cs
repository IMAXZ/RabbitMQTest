using RabbitMQ.Client;
using System;
using System.Text;
using Utils;

namespace P2PSend
{
    class Send
    {
        static void Main(string[] args)
        {
            var factory = ConnectionFactoryUtils.GetConnectionFactory();
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
