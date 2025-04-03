namespace UniGate.ServiceBus;

public interface IMessageBus
{
    Task Publish<T>(T message, string queueName);
}