namespace FirebotProxy.Api.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseFirebotRequestMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<FirebotRequestMiddleware>();
    }
}