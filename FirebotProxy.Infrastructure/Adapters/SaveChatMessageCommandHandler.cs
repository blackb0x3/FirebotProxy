using System.Runtime.CompilerServices;
using FirebotProxy.Data.Access;
using FirebotProxy.SecondaryPorts.SaveChatMessage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("FirebotProxy.Infrastructure.Tests")]

namespace FirebotProxy.Infrastructure.Adapters;

internal class SaveChatMessageCommandHandler : IRequestHandler<SaveChatMessageCommand>
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
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await _context.AddAsync(request.ChatMessage, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        });

        return Unit.Value;
    }
}