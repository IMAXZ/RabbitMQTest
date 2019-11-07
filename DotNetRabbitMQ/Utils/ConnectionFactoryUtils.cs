using RabbitMQ.Client;
using System;

namespace Utils
{
    public class ConnectionFactoryUtils
    {
        public static ConnectionFactory GetConnectionFactory()
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
            return factory;
        }
    }
}
