using FirebotProxy.Data.Entities;
using FirebotProxy.Extensions;
using FirebotProxy.SecondaryPorts.GetChatMessageLeaderboard;
using MediatR;

namespace FirebotProxy.Domain.Tests.FakeHandlers;

internal class FakeGetChatMessageLeaderboardQueryHandler : IRequestHandler<GetChatMessageLeaderboardQuery, IQueryable<KeyValuePair<string, int>>>
{
    private readonly List<ChatMessage> ChatMessages = new()
    {
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer2", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer3", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer4", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() },
        new ChatMessage { SenderUsername = "test_viewer5", Content = Guid.NewGuid().ToString(), Timestamp = DateTimeExtensions.Random() }
    };

    private readonly bool _shouldThrow;

    public FakeGetChatMessageLeaderboardQueryHandler(bool shouldThrow)
    {
        _shouldThrow = shouldThrow;
    }

    public async Task<IQueryable<KeyValuePair<string, int>>> Handle(GetChatMessageLeaderboardQuery request, CancellationToken cancellationToken)
    {
        if (_shouldThrow)
        {
            throw new Exception();
        }

        var x = ChatMessages.AsQueryable()
            .GroupBy(cm => cm.SenderUsername)
            .OrderByDescending(grp => grp.Count())
            .Select(grp => new KeyValuePair<string, int>(grp.Key, grp.Count()));

        return await Task.FromResult(x);
    }
}