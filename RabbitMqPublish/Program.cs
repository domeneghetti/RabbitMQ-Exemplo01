using System;
using RabbitMQ.Client;

namespace RabbitMq
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(@"amqp://guest:guest@127.0.0.1:5672/teste"),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                AutomaticRecoveryEnabled = true
            };

            using(var connection = connectionFactory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "amq.fanout", type: ExchangeType.Fanout, durable: true, autoDelete: false);

                    byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("{{ nome:\"Fernando\", dataNascimento:\"29-07\" }}");
                    
                    channel.BasicPublish(exchange: "amq.fanout",
                                         routingKey: "",
                                         basicProperties: null,
                                         body: messageBodyBytes);

                    Console.WriteLine("Publish!");
                }
            }
        }
    }
}
