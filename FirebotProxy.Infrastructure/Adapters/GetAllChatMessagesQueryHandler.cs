using FirebotProxy.Data.Access;
using FirebotProxy.Data.Entities;
using FirebotProxy.SecondaryPorts.GetAllChatMessages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FirebotProxy.Infrastructure.Adapters;

internal class GetAllChatMessagesQueryHandler : IRequestHandler<GetAllChatMessagesQuery, IReadOnlyCollection<ChatMessage>>
{
    private readonly ILogger<GetAllChatMessagesQueryHandler> _logger;
    private readonly FirebotProxyContext _context;

    public GetAllChatMessagesQueryHandler(ILogger<GetAllChatMessagesQueryHandler> logger, FirebotProxyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IReadOnlyCollection<ChatMessage>> Handle(GetAllChatMessagesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ChatMessages.ToListAsync(cancellationToken: cancellationToken);
    }
}