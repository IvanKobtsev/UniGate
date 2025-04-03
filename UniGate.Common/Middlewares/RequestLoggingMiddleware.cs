using System.Text;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace UniGate.Common.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        context.Request.EnableBuffering();

        var requestBody = await ReadRequestBody(context);
        Log.Information("Request: {Method} {Path} | Query: {Query} | Body: {Body}",
            context.Request.Method, context.Request.Path, context.Request.QueryString, requestBody);

        await next(context);
    }

    private static async Task<string> ReadRequestBody(HttpContext context)
    {
        try
        {
            if (context.Request.ContentLength == null || context.Request.ContentLength == 0)
                return "(empty)";

            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            return body;
        }
        catch (Exception ex)
        {
            Log.Warning("Could not read request body: {Error}", ex.Message);
            return "(unreadable)";
        }
    }
}