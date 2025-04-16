using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace UniGate.Common.HMAC;

public class HmacAuthFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var authHandler = context.HttpContext.RequestServices.GetRequiredService<HmacAuthHandler>();

        var isValid = await authHandler.IsRequestValidAsync(context.HttpContext.Request);

        if (!isValid)
        {
            const string message = "Invalid HMAC signature";
            context.Result = new UnauthorizedObjectResult(new { message });
            return;
        }

        await next();
    }
}