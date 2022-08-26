﻿using FirebotProxy.Domain.Adapters;
using FirebotProxy.Domain.PrimaryPorts.GetViewerChatRank;
using FirebotProxy.Domain.Tests.FakeHandlers;
using FirebotProxy.TestBase;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace FirebotProxy.Domain.Tests;

[TestFixture]
public class A_GetViewerChatRank_Request_Handler : TestFixtureBase
{
    private IMediator _mediator;

    [Test]
    public async Task Returns_A_GetViewerChatRank_Response()
    {
        _mediator = new MediatRFactory(typeof(A_GetViewerChatRank_Request_Handler).Assembly)
            .AddTransientHandler(new FakeGetAllChatMessagesQueryHandler(false))
            .Build();
    }

    [Test]
    public async Task Returns_An_Error_Representation_Upon_Catching_An_Exception()
    {
        _mediator = new MediatRFactory(typeof(A_GetViewerChatRank_Request_Handler).Assembly)
            .AddTransientHandler(new FakeGetAllChatMessagesQueryHandler(true))
            .Build();

        var handler = new GetViewerChatRankRequestHandler(new NullLogger<GetViewerChatRankRequestHandler>(), _mediator);

        var response = await handler.Handle(new GetViewerChatRankRequest { ViewerUsername = "test_viewer" }, CancellationToken.None);

        response.IsT1.Should().BeTrue();

        response.AsT1.Message.Should().Be("Failed to get chat rank for viewer");
    }
}