using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Serilog;
using UniGate.Common.Exceptions;

namespace UniGate.Common.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BaseException ex)
        {
            Log.Warning("Handled exception: {Message} | Path: {Path}", ex.Message, context.Request.Path);
            await HandleExceptionAsync(context, ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled exception occurred | Path: {Path}", context.Request.Path);
            await HandleExceptionAsync(context, (int)HttpStatusCode.InternalServerError,
                "An unexpected error occurred.");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new { message };
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}