using Microsoft.AspNetCore.Builder;
using UniGate.Common.Middlewares;

namespace UniGate.Common.Extensions;

public static class ResponseLoggingExtension
{
    public static IApplicationBuilder UseResponseLoggingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ResponseLoggingMiddleware>();
    }
}