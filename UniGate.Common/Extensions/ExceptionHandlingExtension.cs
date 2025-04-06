using Microsoft.AspNetCore.Builder;
using UniGate.Common.Middlewares;

namespace UniGate.Common.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}