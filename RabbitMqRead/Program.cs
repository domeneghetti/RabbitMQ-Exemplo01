﻿using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace RabbitMqRead
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(@"amqp://guest:guest@127.0.0.1:5672/"),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                AutomaticRecoveryEnabled = true
            };

            using(var connection = connectionFactory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "testeQueueEx", 
                                        durable: true, 
                                        exclusive: false, 
                                        autoDelete: false, 
                                        arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine("[x] Message: {0}", message);
                    };

                    channel.BasicConsume(queue: "testeQueueEx", autoAck: true, consumer: consumer);
                };
            };

            Console.WriteLine("Read!");
        }
    }
}
