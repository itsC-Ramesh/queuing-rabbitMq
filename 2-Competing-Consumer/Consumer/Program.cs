using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Uri uri = new Uri("broker-address:port");

var factory = new ConnectionFactory
{
    HostName = "broker-address:port",

    UserName = "root",
    Password = "root"
};

factory.Ssl.Enabled = true;
factory.Uri = uri;
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "basicTest",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

var consumer = new EventingBasicConsumer(channel);

var random = new Random();

consumer.Received += (model, ea) =>
{
    var processingTime = random.Next(1, 6);

    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

    Console.WriteLine($"Processed - {message} - Taking {processingTime} time to process.");

    Task.Delay(TimeSpan.FromSeconds(processingTime)).Wait();

    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
};

channel.BasicConsume(queue: "basicTest", autoAck: false, consumer: consumer);

Console.ReadKey();
