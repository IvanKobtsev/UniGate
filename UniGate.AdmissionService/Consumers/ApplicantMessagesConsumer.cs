using System.Text.Json;
using UniGate.ServiceBus.DTOs;
using UniGate.ServiceBus.Interfaces;
using UniGate.ServiceBus.Services;
using UniGateAPI.Interfaces;

namespace UniGateAPI.Consumers;

public class ApplicantMessageConsumer(IRabbitMqConnection connectionProvider, IServiceProvider serviceProvider)
    : MessageConsumerBase(connectionProvider, serviceProvider)
{
    protected override string QueueName => "actions-with-applicants";
    protected override string ConsumerTag => "applicant";

    protected override async Task HandleMessageAsync(MessageWrapper<JsonElement> message)
    {
        using var scope = ServiceProvider.CreateScope();
        var backgroundTaskServiceService = scope.ServiceProvider.GetRequiredService<IBackgroundTaskService>();

        switch (message.Action)
        {
            case "UpdateApplicant":
                var applicantRefData = message.Data.Deserialize<UpdateApplicantDto>();

                if (applicantRefData is null)
                    throw new NullReferenceException("UpdateApplicantDto is null");

                await backgroundTaskServiceService.UpdateApplicantName(applicantRefData.UserId,
                    applicantRefData.FullName);

                break;
            default:
                throw new Exception("Unknown action when receiving message in " + QueueName + ": {message.Action}");
        }
    }
}