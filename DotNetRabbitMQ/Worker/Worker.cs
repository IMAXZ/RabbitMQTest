using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using Utils;

namespace Worker
{
    class Worker
    {
        static void Main(string[] args)
        {
            var factory = ConnectionFactoryUtils.GetConnectionFactory();
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "task_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );
                //解开注释则与下方手动确认ack联合可以根据消息处理能力分发给其他消费者
                //channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                Console.WriteLine(" [*] Waiting for message.");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                    Thread.Sleep(2 * 1000);
                    Console.WriteLine(" [x] Done");
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };
                channel.BasicConsume(queue:"task_queue",
                    autoAck:false,
                    consumer:consumer
                    );
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
