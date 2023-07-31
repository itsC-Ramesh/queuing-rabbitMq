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

channel.QueueDeclare(queue: "basicTest", durable: false, exclusive: false, autoDelete: false, arguments: null);

var random = new Random();
int msgId = 1;
while (true)
{
    var pubTime = random.Next(1, 3);

    var message = $"This is message {msgId}";

    var encodedMsg = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("", "basicTest", null, encodedMsg);

    Console.WriteLine($"Published Message: {message}");

    await Task.Delay(TimeSpan.FromSeconds(pubTime));
    
    msgId++;
}