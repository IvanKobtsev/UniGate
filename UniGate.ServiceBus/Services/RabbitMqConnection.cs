using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using UniGate.ServiceBus.Interfaces;

namespace UniGate.ServiceBus.Services;

public class RabbitMqConnection(IConfiguration configuration) : IRabbitMqConnection
{
    private readonly ConnectionFactory _factory = new()
    {
        HostName = configuration["RabbitMq:Host"] ?? "localhost", UserName = "guest", Password = "guest"
    };

    private IConnection? _connection;

    public async Task<IConnection> GetConnection()
    {
        if (_connection is not { IsOpen: true }) _connection = await _factory.CreateConnectionAsync();

        return _connection;
    }
}