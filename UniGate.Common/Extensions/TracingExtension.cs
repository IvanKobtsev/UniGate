using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace UniGate.Common.Extensions;

public static class TracingExtensions
{
    public static IServiceCollection AddCommonOpenTelemetry(this IServiceCollection services, string serviceName)
    {
        services.AddOpenTelemetry().WithTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddJaegerExporter(options =>
                {
                    options.AgentHost = "localhost";
                    options.AgentPort = 6831;
                })
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName));
        });

        return services;
    }
}