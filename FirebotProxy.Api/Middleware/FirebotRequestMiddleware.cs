using System.Net;
using System.Text;

namespace FirebotProxy.Api.Middleware;

public class FirebotRequestMiddleware : IMiddleware
{
    private const string FirebotUserAgent = "Firebot v5 - HTTP Request Effect";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var unauthorizedResponseMessageBuilder = new StringBuilder();

        CheckFirebotUserAgentIsProvided(context, unauthorizedResponseMessageBuilder);

        if (unauthorizedResponseMessageBuilder.Length > 0)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(unauthorizedResponseMessageBuilder.ToString());
            }
        }
        else
        {
            // continue the request if firebot auth checks out
            await next(context);
        }
    }

    private void CheckFirebotUserAgentIsProvided(HttpContext context, StringBuilder unauthorizedResponseMessageBuilder)
    {
        var providedUserAgent = context.Request.Headers.UserAgent.ToString();

        if (!string.Equals(providedUserAgent, FirebotUserAgent, StringComparison.OrdinalIgnoreCase))
        {
            unauthorizedResponseMessageBuilder.Append("Not a request from Firebot.");
        }
    }
}