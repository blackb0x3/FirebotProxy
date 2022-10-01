using FirebotProxy.Domain.Adapters;
using FirebotProxy.Domain.PrimaryPorts.LogChatMessage;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Domain.Tests.FakeHandlers;
using FirebotProxy.Domain.Validators;
using FirebotProxy.TestBase;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace FirebotProxy.Domain.Tests;

public abstract class LogChatMessageRequestHandlerTestsBase
{
    protected static LogChatMessageRequest ConstructRequest(string messageContent, string senderUsername, DateTime timestamp)
    {
        return new LogChatMessageRequest
        {
            Content = messageContent,
            SenderUsername = senderUsername,
            Timestamp = timestamp
        };
    }
}

[TestFixture]
public class A_LogChatMessage_Request_Handler_Logs_A_Chat_Message : LogChatMessageRequestHandlerTestsBase
{
    [Test]
    public async Task Successfully()
    {
        var mediator = new MediatRFactory(typeof(A_LogChatMessage_Request_Handler_Logs_A_Chat_Message).Assembly)
            .AddSingletonHandler(new FakeSaveChatMessageCommandHandler(false));

        var handler = new LogChatMessageRequestHandler(new NullLogger<LogChatMessageRequestHandler>(), new LogChatMessageRequestValidator(), mediator.Build());

        var request = ConstructRequest("test message", "test_viewer", DateTime.UtcNow.AddHours(-1));
        var response = await handler.Handle(request, CancellationToken.None);

        response.Value.Should().BeOfType<LogChatMessageSuccess>();
    }
}

[TestFixture]
public class A_LogChatMessage_Request_Handler_Does_Not_Log_A_Chat_Message : LogChatMessageRequestHandlerTestsBase
{
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public async Task When_The_Message_Content_Is_Not_Provided(string messageContent)
    {
        var mediator = new MediatRFactory(typeof(A_LogChatMessage_Request_Handler_Logs_A_Chat_Message).Assembly)
            .AddSingletonHandler(new FakeSaveChatMessageCommandHandler(false));

        var handler = new LogChatMessageRequestHandler(new NullLogger<LogChatMessageRequestHandler>(), new LogChatMessageRequestValidator(), mediator.Build());

        var request = ConstructRequest(messageContent, "test_viewer", DateTime.UtcNow.AddHours(-1));
        var response = await handler.Handle(request, CancellationToken.None);

        response.Value.Should().BeOfType<ValidationRepresentation>();
        response.AsT1.Errors.Should().HaveCount(1);
        response.AsT1.Errors.First().Should().Be("Content : Message content must not be null or empty.");
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public async Task When_The_Sender_Username_Is_Not_Provided(string senderUsername)
    {
        var mediator = new MediatRFactory(typeof(A_LogChatMessage_Request_Handler_Logs_A_Chat_Message).Assembly)
            .AddSingletonHandler(new FakeSaveChatMessageCommandHandler(false));

        var handler = new LogChatMessageRequestHandler(new NullLogger<LogChatMessageRequestHandler>(), new LogChatMessageRequestValidator(), mediator.Build());

        var request = ConstructRequest("test message", senderUsername, DateTime.UtcNow.AddHours(-1));
        var response = await handler.Handle(request, CancellationToken.None);

        response.Value.Should().BeOfType<ValidationRepresentation>();
        response.AsT1.Errors.Should().HaveCount(1);
        response.AsT1.Errors.First().Should().Be("SenderUsername : Sender username must not be null or empty.");
    }

    [Test]
    public async Task When_An_Exception_Is_Thrown()
    {
        var mediator = new MediatRFactory(typeof(A_LogChatMessage_Request_Handler_Logs_A_Chat_Message).Assembly)
            .AddSingletonHandler(new FakeSaveChatMessageCommandHandler(true));

        var handler = new LogChatMessageRequestHandler(new NullLogger<LogChatMessageRequestHandler>(), new LogChatMessageRequestValidator(), mediator.Build());

        var request = ConstructRequest("test message", "test_viewer", DateTime.UtcNow.AddHours(-1));
        var response = await handler.Handle(request, CancellationToken.None);

        response.Value.Should().BeOfType<ErrorRepresentation>();
        response.AsT2.Message.Should().Be("Failed to log chat message");
    }
}