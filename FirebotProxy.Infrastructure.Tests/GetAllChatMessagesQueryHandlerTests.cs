using FirebotProxy.Data.Access;
using FirebotProxy.Data.Entities;
using FirebotProxy.Infrastructure.Adapters;
using FirebotProxy.SecondaryPorts.GetAllChatMessages;
using FirebotProxy.TestBase;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace FirebotProxy.Infrastructure.Tests;

[TestFixture]
public class A_GetAllChatMessages_Query_Handler : TestFixtureBase
{
    private FirebotProxyContext _context;

    [Test]
    public async Task Returns_A_List_Of_Chat_Messages()
    {
        _context = FakeContextGenerator.GenerateFakeContext();

        var messagesToAdd = Rnd.Next(5, 10);

        for (var i = 0; i < messagesToAdd; i++)
        {
            await _context.ChatMessages.AddAsync(new ChatMessage
            {
                SenderUsername = "blah",
                Content = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        var handler = new GetAllChatMessagesQueryHandler(new NullLogger<GetAllChatMessagesQueryHandler>(), _context);

        var resp = await handler.Handle(new GetAllChatMessagesQuery(), CancellationToken.None);

        resp.Count.Should().Be(messagesToAdd);
    }
}