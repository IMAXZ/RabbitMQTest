using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Utils;

namespace ReceiveLogs2
{
    class ReceiveLogs2
    {
        static void Main(string[] args)
        {
            var factory = ConnectionFactoryUtils.GetConnectionFactory();
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
                var queneName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queneName,
                    exchange: "logs",
                    routingKey: ""
                    );
                Console.WriteLine(" [*] Waiting for logs.");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);
                };
                channel.BasicConsume(queue: queneName, autoAck: true, consumer: consumer);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
