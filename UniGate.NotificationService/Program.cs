using UniGate.NotificationService.Consumers;

var builder = Host.CreateApplicationBuilder(args);

// builder.Services.AddSingleton<IMessageBus>(sp => new RabbitMqMessageBus("localhost"));
builder.Services.AddHostedService<AdmissionAssignedConsumer>();

var host = builder.Build();
host.Run();