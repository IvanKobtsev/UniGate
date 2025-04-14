using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using UniGate.ServiceBus.Interfaces;

namespace UniGate.ServiceBus.Services;

public class MessagePublisher(IConfiguration config) : IMessagePublisher
{
    private readonly ConnectionFactory _factory = new()
    {
        HostName = config["RabbitMq:Host"] ?? "localhost", UserName = "guest", Password = "guest"
    };

    public async Task Publish<T>(T message, string queueName)
    {
        var connection = await _factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queueName, true, false, false);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await channel.BasicPublishAsync(string.Empty, queueName, body);
    }
}