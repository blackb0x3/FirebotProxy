using FirebotProxy.Data.Access;
using FirebotProxy.Data.Entities;
using FirebotProxy.SecondaryPorts.GetChatMessages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FirebotProxy.Infrastructure.Adapters;

internal class GetChatMessagesBySenderQueryHandler : IRequestHandler<GetChatMessagesBySenderQuery, IReadOnlyCollection<ChatMessage>>
{
    private readonly ILogger<GetChatMessagesBySenderQueryHandler> _logger;
    private readonly FirebotProxyContext _context;

    public GetChatMessagesBySenderQueryHandler(ILogger<GetChatMessagesBySenderQueryHandler> logger, FirebotProxyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IReadOnlyCollection<ChatMessage>> Handle(GetChatMessagesBySenderQuery request, CancellationToken cancellationToken)
    {
        return await _context.ChatMessages
            .Where(cm => string.Equals(request.SenderUsername, cm.SenderUsername))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}