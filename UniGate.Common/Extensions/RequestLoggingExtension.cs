using Microsoft.AspNetCore.Builder;
using UniGate.Common.Middlewares;

namespace UniGate.Common.Extensions;

public static class RequestLoggingExtension
{
    public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }
}