﻿using FirebotProxy.Data.Access;
using FirebotProxy.SecondaryPorts.RemoveExpiredChatMessages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FirebotProxy.Infrastructure.Adapters;

internal class RemoveExpiredChatMessagesCommandHandler : IRequestHandler<RemoveExpiredChatMessagesCommand, RemoveExpiredChatMessagesResult>
{
    private readonly ILogger<RemoveExpiredChatMessagesCommandHandler> _logger;
    private readonly FirebotProxyContext _context;

    public RemoveExpiredChatMessagesCommandHandler(ILogger<RemoveExpiredChatMessagesCommandHandler> logger, FirebotProxyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<RemoveExpiredChatMessagesResult> Handle(RemoveExpiredChatMessagesCommand request, CancellationToken cancellationToken)
    {
        var result = new RemoveExpiredChatMessagesResult();
        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var query = $"DELETE FROM ChatMessages WHERE DATE([Timestamp]) > DATE('{request.Cutoff:s}')";

                result.MessagesRemoved = await _context.Database.ExecuteSqlRawAsync(query, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        });

        return result;
    }
}