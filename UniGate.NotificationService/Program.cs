using UniGate.NotificationService.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<AdmissionAssignedConsumer>();

var host = builder.Build();
host.Run();