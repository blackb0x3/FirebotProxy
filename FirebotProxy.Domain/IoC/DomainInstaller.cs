﻿using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FirebotProxy.Domain.IoC;

public class DomainInstaller
{
    private static readonly Assembly DomainProjectAssembly = typeof(DomainInstaller).Assembly;

    public static void Install(IServiceCollection services)
    {
        services.AddMediatR(DomainProjectAssembly);
    }
}