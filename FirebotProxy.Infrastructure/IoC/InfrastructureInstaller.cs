using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FirebotProxy.Infrastructure.IoC;

public class InfrastructureInstaller
{
    private static readonly Assembly InfrastructureProjectAssembly = typeof(InfrastructureInstaller).Assembly;

    public static void Install(IServiceCollection services)
    {
        AddInfrastructureServices(services);

        services.AddMediatR(InfrastructureProjectAssembly);
    }

    private static void AddInfrastructureServices(IServiceCollection services)
    {
    }
}