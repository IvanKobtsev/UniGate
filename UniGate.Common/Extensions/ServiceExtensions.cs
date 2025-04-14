using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using UniGate.Common.Filters;

namespace UniGate.Common.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = ClaimTypes.Role,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"] ??
                                                                        throw new InvalidOperationException(
                                                                            "Jwt:Secret is missing in configuration.")))
                };
            });
        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddAuthSwaggerGen(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(configuration["OpenApiInfo:Version"],
                new OpenApiInfo
                    { Title = configuration["OpenApiInfo:Title"], Version = configuration["OpenApiInfo:Version"] });
            options.EnableAnnotations();
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer <your_token>'"
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
            options.SchemaFilter<OpenApiSchemaFilter>();
        });

        return services;
    }

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

    public static IServiceCollection AddControllersWithJsonSerializers(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                x.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        return services;
    }
}