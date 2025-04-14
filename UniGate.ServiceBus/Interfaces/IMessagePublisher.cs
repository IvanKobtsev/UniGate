namespace UniGate.ServiceBus.Interfaces;

public interface IMessagePublisher
{
    Task Publish<T>(T message, string queueName);
}