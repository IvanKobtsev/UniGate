using Microsoft.EntityFrameworkCore;
using Serilog;
using UniGate.Common.Extensions;
using UniGate.Common.HMAC;
using UniGate.Common.Logging;
using UniGate.ServiceBus.Interfaces;
using UniGate.ServiceBus.Services;
using UniGateAPI.Consumers;
using UniGateAPI.Data;
using UniGateAPI.Interfaces;
using UniGateAPI.Repositories;
using UniGateAPI.Services;

SerilogLogger.ConfigureLogging();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithJsonSerializers();
builder.Services.AddAuthSwaggerGen(builder.Configuration);
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();
builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
builder.Services.AddScoped<IApplicantService, ApplicantService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IAdmissionRepository, AdmissionRepository>();
builder.Services.AddScoped<IBackgroundTaskService, BackgroundTaskService>();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<HmacHttpClient>();
builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
builder.Services.AddHostedService<ApplicantMessageConsumer>();
builder.Services.AddHostedService<ManagerMessageConsumer>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseRequestLoggingMiddleware();
app.UseResponseLoggingMiddleware();
app.UseExceptionHandlingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();