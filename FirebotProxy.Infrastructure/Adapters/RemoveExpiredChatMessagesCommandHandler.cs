using System.Runtime.CompilerServices;
using FirebotProxy.Data.Access;
using FirebotProxy.SecondaryPorts.RemoveExpiredChatMessages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("FirebotProxy.Infrastructure.Tests")]

namespace FirebotProxy.Infrastructure.Adapters;

internal class RemoveExpiredChatMessagesCommandHandler : IRequestHandler<RemoveExpiredChatMessagesCommand>
{
    private readonly ILogger<RemoveExpiredChatMessagesCommandHandler> _logger;
    private readonly FirebotProxyContext _context;

    public RemoveExpiredChatMessagesCommandHandler(ILogger<RemoveExpiredChatMessagesCommandHandler> logger, FirebotProxyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Unit> Handle(RemoveExpiredChatMessagesCommand request, CancellationToken cancellationToken)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                if (_context.Database.IsRelational())
                {
                    var formattedCutoff = request.Cutoff.ToString("yyyy-MM-dd HH:mm:ss");
                    var query = $"DELETE FROM ChatMessages WHERE DATE(Timestamp) < DATE('{formattedCutoff}')";
                    await _context.Database.ExecuteSqlRawAsync(query, cancellationToken);
                }

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