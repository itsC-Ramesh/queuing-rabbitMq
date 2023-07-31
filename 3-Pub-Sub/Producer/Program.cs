using System;
using System.Text;
using RabbitMQ.Client;

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

channel.ExchangeDeclare(exchange: "testPubSub", type: ExchangeType.Fanout);

var message = "Broadcasting this message!!";

var encodedMsg = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange:"testPubSub", "", null, encodedMsg);

Console.WriteLine($"Published Message: {message}");
