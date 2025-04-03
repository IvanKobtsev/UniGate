using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace UniGate.ServiceBus;

public class RabbitMqMessageBus(string rabbitMqHost) : IMessageBus
{
    private readonly ConnectionFactory _factory = new()
        { HostName = rabbitMqHost, UserName = "guest", Password = "guest" };

    public async Task Publish<T>(T message, string queueName)
    {
        await using var connection = await _factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queueName, false, false, false);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await channel.BasicPublishAsync(string.Empty, queueName, body);
    }
}