using FirebotProxy.Data.Access;
using FirebotProxy.Data.Entities;
using FirebotProxy.Infrastructure.Adapters;
using FirebotProxy.SecondaryPorts.RemoveExpiredChatMessages;
using FirebotProxy.TestBase;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace FirebotProxy.Infrastructure.Tests;

[TestFixture]
public class A_RemoveExpiredChatMessages_Command_Handler : TestFixtureBase
{
    private FirebotProxyContext _context;

    [Test]
    public async Task Removes_An_Expected_Number_Of_Expired_Chat_Messages()
    {
        var cutoff = DateTime.UtcNow;
        var messagesToAdd = Rnd.Next(5, 10);
        var expectedRemovedMessages = 0;

        _context = FakeContextGenerator.GenerateFakeContext();

        for (var i = 0; i < messagesToAdd; i++)
        {
            var msg = new ChatMessage { Content = Guid.NewGuid().ToString(), SenderUsername = "test_viewer" };

            if (Rnd.NextDouble() >= 0.5)
            {
                msg.Timestamp = cutoff.AddDays(1);
                expectedRemovedMessages++;
            }
            else
            {
                msg.Timestamp = cutoff.AddDays(-1);
            }

            await _context.ChatMessages.AddAsync(msg);
            await _context.SaveChangesAsync();
        }

        var handler = new RemoveExpiredChatMessagesCommandHandler(new NullLogger<RemoveExpiredChatMessagesCommandHandler>(), _context);

        Func<Task> act = async () =>
        {
            await handler.Handle(new RemoveExpiredChatMessagesCommand { Cutoff = cutoff }, CancellationToken.None);
        };

        await act.Should().NotThrowAsync<Exception>();
    }
}