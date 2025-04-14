namespace UniGate.ServiceBus.DTOs;

public class MessageWrapper<T>
{
    public required string Action { get; set; }
    public T? Data { get; set; }
}