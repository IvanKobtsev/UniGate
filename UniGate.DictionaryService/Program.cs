using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Quartz;
using Serilog;
using UniGate.Common.Extensions;
using UniGate.Common.Logging;
using UniGate.DictionaryService.Data;
using UniGate.DictionaryService.ExternalApiClient;
using UniGate.DictionaryService.Interfaces;
using UniGate.DictionaryService.Jobs;
using UniGate.DictionaryService.Repositories;
using UniGate.DictionaryService.Services;

SerilogLogger.ConfigureLogging();

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
builder.Services.AddSwaggerGen();

builder.Services.Configure<ExternalApiOptions>(
    builder.Configuration.GetSection("ExternalApi"));

builder.Services.AddScoped<IDictionaryRepository, DictionaryRepository>();
builder.Services.AddScoped<IDictionaryService, DictionaryService>();
builder.Services.AddScoped<IExternalApiClient, ExternalApiClient>();
builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<HttpClient>();

builder.Services.AddQuartz(q =>
{
    q.AddJob<DictionaryImportJob>(opts =>
    {
        opts.WithIdentity("DictionaryImportJob");
        opts.StoreDurably();
    });
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

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