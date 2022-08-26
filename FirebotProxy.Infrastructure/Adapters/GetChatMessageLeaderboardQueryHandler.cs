using System.Runtime.CompilerServices;
using FirebotProxy.Data.Access;
using FirebotProxy.Data.Entities;
using FirebotProxy.SecondaryPorts.GetChatMessageLeaderboard;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("FirebotProxy.Infrastructure.Tests")]

namespace FirebotProxy.Infrastructure.Adapters;

internal class GetChatMessageLeaderboardQueryHandler : IRequestHandler<GetChatMessageLeaderboardQuery, IOrderedQueryable<KeyValuePair<string, int>>>
{
    private readonly ILogger<GetChatMessageLeaderboardQueryHandler> _logger;
    private readonly FirebotProxyContext _context;

    public GetChatMessageLeaderboardQueryHandler(ILogger<GetChatMessageLeaderboardQueryHandler> logger, FirebotProxyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IOrderedQueryable<KeyValuePair<string, int>>> Handle(GetChatMessageLeaderboardQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(
            _context.ChatMessages
                .GroupBy(cm => cm.SenderUsername)
                .Select(grp => new KeyValuePair<string, int>(grp.Key, grp.Count()))
                .OrderByDescending(kvp => kvp.Value)
        );
    }
}