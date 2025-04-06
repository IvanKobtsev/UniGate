using System.Text;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace UniGate.Common.Middlewares;

public class ResponseLoggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;
        await using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        try
        {
            await next(context);
            var responseBody = await ReadResponseBody(context);
            Log.Information("Result: {StatusCode} {Path} | Body: {Body}",
                context.Response.StatusCode, context.Request.Path, responseBody);
        }
        finally
        {
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);
        }
    }

    private static async Task<string> ReadResponseBody(HttpContext context)
    {
        try
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Response.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            return string.IsNullOrWhiteSpace(body) ? "(empty)" : body;
        }
        catch (Exception ex)
        {
            Log.Warning("Could not read response body: {Error}", ex.Message);
            return "(unreadable)";
        }
    }
}