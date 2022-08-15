using FirebotProxy.Data.Access;
using FirebotProxy.SecondaryPorts.RemoveMessagesByUsername;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FirebotProxy.Infrastructure.Adapters;

internal class RemoveMessagesByUsernameCommandHandler : IRequestHandler<RemoveMessagesByUsernameCommand>
{
    private readonly ILogger<RemoveMessagesByUsernameCommandHandler> _logger;
    private readonly FirebotProxyContext _context;

    public RemoveMessagesByUsernameCommandHandler(ILogger<RemoveMessagesByUsernameCommandHandler> logger, FirebotProxyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Unit> Handle(RemoveMessagesByUsernameCommand request, CancellationToken cancellationToken)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                const string query = "DELETE FROM ChatMessages WHERE SenderUsername = {0}";

                await _context.Database.ExecuteSqlRawAsync(query, request.SenderUsername);

                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        });

        return Unit.Value;
    }
}