using FirebotProxy.Data.Access;
using FirebotProxy.Data.Entities;
using FirebotProxy.Infrastructure.Adapters;
using FirebotProxy.SecondaryPorts.RemoveMessagesByUsername;
using FirebotProxy.TestBase;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace FirebotProxy.Infrastructure.Tests;

[TestFixture]
public class A_RemoveMessagesByUsername_Command_Handler : TestFixtureBase
{
    private FirebotProxyContext _context = null!;

    [Test]
    public async Task Removes_An_Expected_Number_Of_Chat_Messages_Sent_By_User()
    {
        const string usernameToCheckFor = "test_viewer";
        var messagesToAdd = Rnd.Next(5, 10);
        var testViewerSpecificMessages = 0;

        _context = FakeContextGenerator.GenerateFakeContext();

        for (var i = 0; i < messagesToAdd; i++)
        {
            var msg = new ChatMessage { Content = Guid.NewGuid().ToString(), Timestamp = DateTime.UtcNow };

            if (Rnd.NextDouble() >= 0.5)
            {
                msg.SenderUsername = usernameToCheckFor;
                testViewerSpecificMessages++;
            }
            else
            {
                msg.SenderUsername = "blah";
            }

            await _context.ChatMessages.AddAsync(msg);
            await _context.SaveChangesAsync();
        }

        var handler = new RemoveMessagesByUsernameCommandHandler(new NullLogger<RemoveMessagesByUsernameCommandHandler>(), _context);

        Func<Task> act = async () =>
        {
            await handler.Handle(new RemoveMessagesByUsernameCommand { SenderUsername = usernameToCheckFor }, CancellationToken.None);
        };

        await act.Should().NotThrowAsync();
    }
}