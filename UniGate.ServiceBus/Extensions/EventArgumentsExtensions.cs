using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using UniGate.ServiceBus.DTOs;

namespace UniGate.ServiceBus.Extensions;

public static class EventArgumentsExtensions
{
    public static MessageWrapper<JsonElement>? ToMessage(this BasicDeliverEventArgs eventArgs)
    {
        var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
        return JsonConvert.DeserializeObject<MessageWrapper<JsonElement>>(json);
    }
}