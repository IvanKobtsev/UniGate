using System.Text.Json;
using UniGate.ServiceBus.DTOs;
using UniGate.ServiceBus.Interfaces;
using UniGate.ServiceBus.Services;
using UniGateAPI.Interfaces;

namespace UniGateAPI.Consumers;

public class ManagerMessageConsumer(IRabbitMqConnection connectionProvider, IServiceProvider serviceProvider)
    : MessageConsumerBase(connectionProvider, serviceProvider)
{
    protected override string QueueName => "actions-with-managers";
    protected override string ConsumerTag => "manager";

    protected override async Task HandleMessageAsync(MessageWrapper<JsonElement> message)
    {
        using var scope = ServiceProvider.CreateScope();
        var backgroundTaskServiceService = scope.ServiceProvider.GetRequiredService<IBackgroundTaskService>();

        switch (message.Action)
        {
            case "UpdateManager":
                var managerRefData = message.Data.Deserialize<UpdateManagerDto>();

                if (managerRefData is null)
                    throw new NullReferenceException("UpdateManagerDto is null");

                await backgroundTaskServiceService.UpdateManagerData(managerRefData.UserId,
                    managerRefData.IsChief,
                    managerRefData.FullName);

                break;
            default:
                throw new Exception($"Unknown action: {message.Action}");
        }
    }
}