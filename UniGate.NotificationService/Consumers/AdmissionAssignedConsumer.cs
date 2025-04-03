using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace UniGate.NotificationService.Consumers;

public class AdmissionAssignedConsumer : BackgroundService
{
    private readonly ConnectionFactory _factory = new()
        { HostName = "localhost", UserName = "guest", Password = "guest" };

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        var connection = await _factory.CreateConnectionAsync(token);
        var channel = await connection.CreateChannelAsync(cancellationToken: token);

        const string queueName = "applicant_queue";
        await channel.QueueDeclareAsync(queueName, false, false, false, cancellationToken: token);

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);


            Console.WriteLine($"Received message: {message}");
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queueName, true, consumer, token);
    }
}