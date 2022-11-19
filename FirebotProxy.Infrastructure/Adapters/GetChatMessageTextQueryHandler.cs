using FirebotProxy.Data.Access;
using FirebotProxy.SecondaryPorts.GetChatMessageText;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FirebotProxy.Infrastructure.Adapters;

public class GetChatMessageTextQueryHandler : IRequestHandler<GetChatMessageTextQuery, string>
{
    private readonly ILogger<GetChatMessageTextQueryHandler> _logger;
    private readonly FirebotProxyContext _context;

    public GetChatMessageTextQueryHandler(ILogger<GetChatMessageTextQueryHandler> logger, FirebotProxyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<string> Handle(GetChatMessageTextQuery request, CancellationToken cancellationToken)
    {
        var chatMessages = _context.ChatMessages.AsQueryable();

        if (request.ViewerUsername != null)
        {
            chatMessages = chatMessages.Where(msg => msg.SenderUsername.Equals(request.ViewerUsername));
        }

        if (request.StreamDate is { })
        {
            chatMessages = chatMessages
                .Where(msg => msg.Timestamp >= request.StreamDate.Value)
                .Where(msg => msg.Timestamp < request.StreamDate.Value.AddDays(1));
        }

        var chatText = string.Join(" ", chatMessages.Select(msg => msg.Content));

        return await Task.FromResult(chatText);
    }
}