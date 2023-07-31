using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Uri uri = new Uri("broker-address:port");

var factory = new ConnectionFactory
{
    HostName = "broker-address:port",

    UserName = "uname",
    Password = "password"
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

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
    Console.WriteLine($"Message Recived => {message}");
};

channel.BasicConsume(queue:"basicTest", autoAck: true, consumer: consumer);

Console.ReadKey();
