using FirebotProxy.Data.Access;
using FirebotProxy.Data.Entities;
using FirebotProxy.Infrastructure.Adapters;
using FirebotProxy.SecondaryPorts.GetChatMessages;
using FirebotProxy.TestBase;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace FirebotProxy.Infrastructure.Tests;

[TestFixture]
public class A_GetChatMessagesBySender_Query_Handler : TestFixtureBase
{
    private FirebotProxyContext _context;

    [Test]
    public async Task Returns_A_List_Of_Chat_Messages_Filtered_By_Viewer_Username()
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

        var handler = new GetChatMessagesBySenderQueryHandler(new NullLogger<GetChatMessagesBySenderQueryHandler>(), _context);

        var resp = await handler.Handle(new GetChatMessagesBySenderQuery { SenderUsername = usernameToCheckFor }, CancellationToken.None);

        resp.Count.Should().Be(testViewerSpecificMessages);
    }
}