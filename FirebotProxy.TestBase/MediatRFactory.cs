﻿using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FirebotProxy.TestBase;

public class MediatRFactory
{
    private readonly IServiceCollection _services;

    private MediatRFactory()
    {
        _services = new ServiceCollection();
    }

    public MediatRFactory(Assembly assembly) : this()
    {
        _services.AddMediatR(assembly);
    }

    public MediatRFactory AddTransientHandler<T, T2>(IRequestHandler<T, T2> instance) where T : class, IRequest<T2>
    {
        _services.AddTransient((s) => instance);
        return this;
    }

    public MediatRFactory AddTransientType<T, T2>()
    {
        _services.AddTransient(typeof(T), typeof(T2));
        return this;
    }

    public MediatRFactory AddTransientType<T>(T instance) where T : class
    {
        _services.AddTransient((s) => instance);
        return this;
    }

    public IMediator Build()
    {
        var provider = _services.BuildServiceProvider();

        return provider.GetRequiredService<IMediator>();
    }
}