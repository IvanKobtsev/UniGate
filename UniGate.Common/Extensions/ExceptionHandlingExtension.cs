using Microsoft.AspNetCore.Builder;
using UniGate.Common.Middlewares;

namespace UniGate.Common.Extensions;

public static class ExceptionHandlingExtension
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}