using FirebotProxy.Api.Middleware;

namespace FirebotProxy.Api;

public class ApiInstaller
{
    public static void Install(IServiceCollection services)
    {
        services.AddTransient<FirebotRequestMiddleware>();
    }
}