using FirebotProxy.Data.Access;
using FirebotProxy.Data.Entities;
using FirebotProxy.Infrastructure.Adapters;
using FirebotProxy.SecondaryPorts.SaveChatMessage;
using FirebotProxy.TestBase;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace FirebotProxy.Infrastructure.Tests;

[TestFixture]
public class A_SaveChatMessage_Command_Handler : TestFixtureBase
{
    private FirebotProxyContext _context;

    [Test]
    public async Task Saves_A_Chat_Message()
    {
        _context = FakeContextGenerator.GenerateFakeContext();

        var chatMsg = new ChatMessage
        {
            SenderUsername = "test_viewer",
            Timestamp = DateTime.UtcNow,
            Content = Guid.NewGuid().ToString()
        };

        var handler = new SaveChatMessageCommandHandler(new NullLogger<SaveChatMessageCommandHandler>(), _context);

        Func<Task> act = async () =>
        {
            await handler.Handle(new SaveChatMessageCommand { ChatMessage = chatMsg }, CancellationToken.None);
        };

        await act.Should().NotThrowAsync();

        var expectedChatMsg = _context.ChatMessages.FirstOrDefault(cm => cm.SenderUsername == "test_viewer");

        expectedChatMsg.Should().NotBeNull();
    }
}