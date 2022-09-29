using FirebotProxy.Data.Entities;
using FirebotProxy.SecondaryPorts.GetChatMessages;
using MediatR;

namespace FirebotProxy.Domain.Tests.FakeHandlers;

internal class FakeGetChatMessagesBySenderQueryHandler : IRequestHandler<GetChatMessagesBySenderQuery, IReadOnlyCollection<ChatMessage>>
{
    private readonly string _senderUsername;
    private readonly DateTime _startDate;
    private readonly DateTime _endDate;

    public FakeGetChatMessagesBySenderQueryHandler(string senderUsername, DateTime startDate, DateTime endDate)
    {
        _senderUsername = senderUsername;
        _startDate = startDate;
        _endDate = endDate;
    }

    public async Task<IReadOnlyCollection<ChatMessage>> Handle(GetChatMessagesBySenderQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(GenerateMessages().Where(cm => cm.SenderUsername.Equals(_senderUsername)).ToList());
    }

    private IEnumerable<ChatMessage> GenerateMessages()
    {
        for (var d = _startDate; d <= _endDate; d = d.AddDays(1))
        {
            yield return new ChatMessage
            {
                Id = Guid.NewGuid(),
                SenderUsername = _senderUsername,
                Content = "content",
                Timestamp = d
            };
        }
    }
}