using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Uri uri = new Uri("broker-address:port");

var factory = new ConnectionFactory
{
    HostName = "broker-address:port",
    // UserName = "root",
    // Password = "root"

    UserName = "root",
    Password = "root"
};

factory.Ssl.Enabled = true;
factory.Uri = uri;
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "testPubSub", type: ExchangeType.Fanout);

var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueName, exchange: "testPubSub", routingKey: "");


var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
    Console.WriteLine($"Consumer-2 => Message Recived => {message}");

};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

Console.ReadKey();
