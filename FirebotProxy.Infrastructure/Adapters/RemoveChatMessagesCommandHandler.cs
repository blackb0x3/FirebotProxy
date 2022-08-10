using FirebotProxy.Data.Access;
using FirebotProxy.SecondaryPorts.RemoveChatMessages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FirebotProxy.Infrastructure.Adapters;

internal class RemoveChatMessagesCommandHandler : IRequestHandler<RemoveChatMessagesCommand, RemoveChatMessagesResult>
{
    private readonly ILogger<RemoveChatMessagesCommandHandler> _logger;
    private readonly FirebotProxyContext _context;

    public RemoveChatMessagesCommandHandler(ILogger<RemoveChatMessagesCommandHandler> logger, FirebotProxyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<RemoveChatMessagesResult> Handle(RemoveChatMessagesCommand request, CancellationToken cancellationToken)
    {
        var query = $"DELETE FROM ChatMessages WHERE DATE([Timestamp]) > DATE('{request.Cutoff:s}')";

        var rowsRemoved = await _context.Database.ExecuteSqlRawAsync(query, cancellationToken);

        return new RemoveChatMessagesResult { MessagesRemoved = rowsRemoved };
    }
}