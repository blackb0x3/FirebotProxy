using FirebotProxy.Domain.Adapters;
using FirebotProxy.Domain.PrimaryPorts.RemoveExpiredChatMessages;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Domain.Tests.FakeHandlers;
using FirebotProxy.TestBase;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace FirebotProxy.Domain.Tests;

[TestFixture]
public class A_RemoveExpiredChatMessages_Request_Handler_Removes_Expired_Messages
{
    [Test]
    public async Task Successfully()
    {
        var logger = new NullLogger<RemoveExpiredChatMessagesRequestHandler>();

        var mediator = new MediatRFactory(typeof(A_RemoveMessagesOfBannedViewer_Request_Handler_Removes_Messages).Assembly)
            .AddSingletonHandler(new FakeRemoveExpiredChatMessagesCommandHandler(false));

        var handler = new RemoveExpiredChatMessagesRequestHandler(logger, mediator.Build());

        var response = await handler.Handle(new RemoveExpiredChatMessagesRequest(), CancellationToken.None);

        response.Value.Should().BeOfType<RemoveExpiredChatMessagesSuccess>();
    }
}

[TestFixture]
public class A_RemoveExpiredChatMessages_Request_Handler_Does_Not_Remove_Expired_Messages
{
    [Test]
    public async Task When_An_Exception_Is_Thrown()
    {
        var logger = new NullLogger<RemoveExpiredChatMessagesRequestHandler>();

        var mediator = new MediatRFactory(typeof(A_RemoveMessagesOfBannedViewer_Request_Handler_Removes_Messages).Assembly)
            .AddSingletonHandler(new FakeRemoveExpiredChatMessagesCommandHandler(true));

        var handler = new RemoveExpiredChatMessagesRequestHandler(logger, mediator.Build());

        var response = await handler.Handle(new RemoveExpiredChatMessagesRequest(), CancellationToken.None);

        response.Value.Should().BeOfType<ErrorRepresentation>();
        response.AsT1.Message.Should().Be("Failed to remove expired chat messages");
    }
}