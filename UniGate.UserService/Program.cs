using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using StackExchange.Redis;
using UniGate.Common.Extensions;
using UniGate.Common.Logging;
using UniGate.Common.Utilities;
using UniGate.ServiceBus.Interfaces;
using UniGate.ServiceBus.Services;
using UniGate.UserService.Data;
using UniGate.UserService.Interfaces;
using UniGate.UserService.Models;
using UniGate.UserService.Repositories;
using UniGate.UserService.Services;

SerilogLogger.ConfigureLogging();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllersWithJsonSerializers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthSwaggerGen(builder.Configuration);

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection") ??
                                  throw new InvalidOperationException(
                                      "RedisConnection:Connection string is missing in configuration.")));

builder.Services.AddScoped<ITokenStore, RedisTokenStore>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IApplicantService, ApplicantService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRabbitMqConnection, RabbitMqConnection>();
builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers().AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddNewtonsoftJson(x =>
    {
        x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        x.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

var app = builder.Build();

app.UseRequestLoggingMiddleware();
app.UseResponseLoggingMiddleware();
app.UseExceptionHandlingMiddleware();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error applying migrations: {ex.Message}");
    }

    await ExtraTask.SeedRolesAsync(services);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();