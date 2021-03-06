﻿using RabbitMQ.Client;
using System;
using System.Text;
using Utils;

namespace NewTask
{
    class NewTask
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
                //消息设置持久性，即便RabbitMQ重新启动，队列中的消息也不会丢失
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                
                for (int i = 1; i <= 20; i++)
                {
                    var message = GetMessage(args);
                    message += "===>" + i;
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                        routingKey: "task_queue",
                        basicProperties: properties,
                        body: body
                        );
                    Console.WriteLine(" [x] Sent {0}", message);
                }
                
            }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static string GetMessage(string[] args)
        {
            return (args.Length > 0) ? string.Join(" ", args) : "Hello World!";
        }
    }
}
