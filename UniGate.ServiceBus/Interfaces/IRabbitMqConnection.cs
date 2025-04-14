using RabbitMQ.Client;

namespace UniGate.ServiceBus.Interfaces;

public interface IRabbitMqConnection
{
    public Task<IConnection> GetConnection();
}