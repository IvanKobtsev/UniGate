using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using UniGate.ServiceBus.DTOs;
using UniGate.ServiceBus.Interfaces;

namespace UniGate.ServiceBus.Services;

public abstract class MessageConsumerBase(IRabbitMqConnection connectionProvider, IServiceProvider serviceProvider)
    : BackgroundService
{
    protected readonly IServiceProvider ServiceProvider = serviceProvider;
    protected virtual string QueueName { get; set; } = "default-queue";
    protected virtual string ConsumerTag { get; set; } = "default-tag";

    protected abstract Task HandleMessageAsync(MessageWrapper<JsonElement> message);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = await connectionProvider.GetConnection();
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(QueueName, true, false, false, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonSerializer.Deserialize<MessageWrapper<JsonElement>>(json);

            if (message is null) throw new NullReferenceException();

            await HandleMessageAsync(message);
        };

        await channel.BasicConsumeAsync(QueueName, true, consumer: consumer, noLocal: false,
            exclusive: false,
            consumerTag: ConsumerTag, arguments: null, cancellationToken: stoppingToken);
    }
}