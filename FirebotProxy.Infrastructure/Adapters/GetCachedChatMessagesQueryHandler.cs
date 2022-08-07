using FirebotProxy.Data.Entities;
using FirebotProxy.SecondaryPorts.GetCachedChatMessages;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FirebotProxy.Infrastructure.Adapters;

public class GetCachedChatMessagesQueryHandler : IRequestHandler<GetCachedChatMessagesQuery, List<ChatMessage>>
{
    private readonly ILogger<GetCachedChatMessagesQueryHandler> _logger;

    public GetCachedChatMessagesQueryHandler(ILogger<GetCachedChatMessagesQueryHandler> logger)
    {
        _logger = logger;
    }

    public Task<List<ChatMessage>> Handle(GetCachedChatMessagesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}