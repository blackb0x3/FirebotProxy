using FirebotProxy.Data.Entities;
using FirebotProxy.Extensions;
using FirebotProxy.SecondaryPorts.GetAllChatMessages;
using MediatR;

namespace FirebotProxy.Domain.Tests.FakeHandlers;

internal class FakeGetAllChatMessagesQueryHandler : IRequestHandler<GetAllChatMessagesQuery, IReadOnlyCollection<ChatMessage>>
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

    public FakeGetAllChatMessagesQueryHandler(bool shouldThrow)
    {
        _shouldThrow = shouldThrow;
    }

    public async Task<IReadOnlyCollection<ChatMessage>> Handle(GetAllChatMessagesQuery request, CancellationToken cancellationToken)
    {
        if (_shouldThrow)
        {
            throw new Exception();
        }

        return await Task.FromResult(ChatMessages);
    }
}