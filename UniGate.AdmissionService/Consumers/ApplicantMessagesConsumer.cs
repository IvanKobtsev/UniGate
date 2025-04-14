using System.Text;
using System.Text.Json;
using RabbitMQ.Client.Events;
using UniGate.ServiceBus.DTOs;
using UniGate.ServiceBus.Interfaces;
using UniGateAPI.Interfaces;

namespace UniGateAPI.Consumers;

public class ApplicantMessageConsumer(IRabbitMqConnection connectionProvider, IServiceProvider serviceProvider)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = await connectionProvider.GetConnection();
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync("applicant-queue", false, false, false, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonSerializer.Deserialize<MessageWrapper<JsonElement>>(json);

            if (message is null) throw new NullReferenceException();

            using var scope = serviceProvider.CreateScope();
            var backgroundTaskServiceService = scope.ServiceProvider.GetRequiredService<IBackgroundTaskService>();

            switch (message.Action)
            {
                case "RegisteredApplicant":
                    var applicantRefData = message.Data.Deserialize<RegisteredApplicantDto>();

                    if (applicantRefData is null)
                        throw new NullReferenceException("RegisteredApplicantDto is null");

                    await backgroundTaskServiceService.UpdateApplicantName(applicantRefData.UserId,
                        applicantRefData.FullName);

                    break;
                default:
                    throw new Exception($"Unknown action: {message.Action}");
            }
        };

        await channel.BasicConsumeAsync("actions-with-applicants", true, consumer: consumer, noLocal: false,
            exclusive: false,
            consumerTag: "applicant", arguments: null, cancellationToken: stoppingToken);
    }
}