using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FirebotProxy.Domain.IoC;

public class DomainInstaller
{
    public static readonly Assembly DomainProjectAssembly = typeof(DomainInstaller).Assembly;

    public static void Install(IServiceCollection services)
    {
        services.AddMediatR(DomainProjectAssembly);
        services.AddValidatorsFromAssembly(DomainProjectAssembly, ServiceLifetime.Singleton);
    }
}