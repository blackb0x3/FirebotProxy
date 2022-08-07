using FirebotProxy.Data.Access;
using FirebotProxy.SecondaryPorts.SaveChatMessage;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FirebotProxy.Infrastructure.Adapters;

public class SaveChatMessageCommandHandler : IRequestHandler<SaveChatMessageCommand>
{
    private readonly ILogger<SaveChatMessageCommandHandler> _logger;
    private readonly FirebotProxyContext _context;

    public SaveChatMessageCommandHandler(ILogger<SaveChatMessageCommandHandler> logger, FirebotProxyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Unit> Handle(SaveChatMessageCommand request, CancellationToken cancellationToken)
    {
        await _context.AddAsync(request.ChatMessage, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}