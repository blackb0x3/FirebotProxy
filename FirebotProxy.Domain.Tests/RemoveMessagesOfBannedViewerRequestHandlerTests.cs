using FirebotProxy.Domain.Adapters;
using FirebotProxy.Domain.PrimaryPorts.RemoveMessagesOfBannedViewer;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Domain.Tests.FakeHandlers;
using FirebotProxy.Domain.Validators;
using FirebotProxy.TestBase;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace FirebotProxy.Domain.Tests;

public abstract class RemoveMessagesOfBannedViewerRequestHandlerTestBase
{
    protected static RemoveViewerMessagesRequest ConstructRequest(string username) =>
        new()
        {
            ViewerUsername = username
        };
}

[TestFixture]
public class A_RemoveMessagesOfBannedViewer_Request_Handler_Removes_Messages : RemoveMessagesOfBannedViewerRequestHandlerTestBase
{
    [Test]
    public async Task Successfully()
    {
        var logger = new NullLogger<RemoveViewerMessagesRequestHandler>();

        var mediator = new MediatRFactory(typeof(A_RemoveMessagesOfBannedViewer_Request_Handler_Removes_Messages).Assembly)
            .AddSingletonHandler(new FakeRemoveMessagesByUsernameCommandHandler(false));

        var validator = new RemoveMessagesOfBannedViewerRequestValidator();

        var handler = new RemoveViewerMessagesRequestHandler(logger, mediator.Build(), validator);

        var request = ConstructRequest("test_viewer");
        var response = await handler.Handle(request, CancellationToken.None);

        response.Value.Should().BeOfType<RemoveViewerMessagesSuccess>();
    }
}

[TestFixture]
public class A_RemoveMessagesOfBannedViewer_Request_Handler_Does_Not_Remove_Messages : RemoveMessagesOfBannedViewerRequestHandlerTestBase
{
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public async Task When_The_Username_Of_The_Banned_Viewer_Is_Not_Provided(string username)
    {
        var logger = new NullLogger<RemoveViewerMessagesRequestHandler>();

        var mediator = new MediatRFactory(typeof(A_RemoveMessagesOfBannedViewer_Request_Handler_Removes_Messages).Assembly)
            .AddSingletonHandler(new FakeRemoveMessagesByUsernameCommandHandler(false));

        var validator = new RemoveMessagesOfBannedViewerRequestValidator();

        var handler = new RemoveViewerMessagesRequestHandler(logger, mediator.Build(), validator);

        var request = ConstructRequest(username);
        var response = await handler.Handle(request, CancellationToken.None);

        response.Value.Should().BeOfType<ValidationRepresentation>();
        response.AsT1.Errors.Should().HaveCount(1);
        response.AsT1.Errors.First().Should().Be("BannedViewerUsername : Viewer username must not be null or empty.");
    }

    [Test]
    public async Task When_An_Exception_Is_Thrown()
    {
        var logger = new NullLogger<RemoveViewerMessagesRequestHandler>();

        var mediator = new MediatRFactory(typeof(A_RemoveMessagesOfBannedViewer_Request_Handler_Removes_Messages).Assembly)
            .AddSingletonHandler(new FakeRemoveMessagesByUsernameCommandHandler(true));

        var validator = new RemoveMessagesOfBannedViewerRequestValidator();

        var handler = new RemoveViewerMessagesRequestHandler(logger, mediator.Build(), validator);

        var request = ConstructRequest("test_viewer");
        var response = await handler.Handle(request, CancellationToken.None);

        response.Value.Should().BeOfType<ErrorRepresentation>();
        response.AsT2.Message.Should().Be("Failed to remove messages of banned viewer");
    }
}