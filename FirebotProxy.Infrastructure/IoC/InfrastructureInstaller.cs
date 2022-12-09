using System.Reflection;
using FirebotProxy.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Refit;

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
        services.AddRefitClient<IQuickChartApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://quickchart.io"));
    }
}