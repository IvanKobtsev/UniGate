using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using UniGate.Common.Extensions;
using UniGate.ServiceBus;
using UniGateAPI.Data;
using UniGateAPI.Interfaces;
using UniGateAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers().AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddNewtonsoftJson(x =>
    {
        x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        x.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "UniGate.AdmissionService API", Version = "v1" });
    options.EnableAnnotations();
});

builder.Services.AddScoped<IApplicantService, ApplicantService>();
builder.Services.AddSingleton<IMessageBus>(sp => new RabbitMqMessageBus("localhost"));

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