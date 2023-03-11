using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FirebotProxy.Domain.IoC;

public class DomainInstaller
{
    private static readonly Assembly DomainProjectAssembly = typeof(DomainInstaller).Assembly;

    public static void Install(IServiceCollection services)
    {
        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(DomainProjectAssembly);
        });

        services.AddValidatorsFromAssembly(DomainProjectAssembly, ServiceLifetime.Singleton);
    }
}