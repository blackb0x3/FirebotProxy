using System.Runtime.CompilerServices;
using FirebotProxy.Data.Access;
using FirebotProxy.Data.Entities;
using FirebotProxy.SecondaryPorts.GetChatMessageLeaderboard;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("FirebotProxy.Infrastructure.Tests")]

namespace FirebotProxy.Infrastructure.Adapters;

internal class GetChatMessageLeaderboardQueryHandler : IRequestHandler<GetChatMessageLeaderboardQuery, IQueryable<KeyValuePair<string, int>>>
{
    private readonly ILogger<GetChatMessageLeaderboardQueryHandler> _logger;
    private readonly FirebotProxyContext _context;

    public GetChatMessageLeaderboardQueryHandler(ILogger<GetChatMessageLeaderboardQueryHandler> logger, FirebotProxyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IQueryable<KeyValuePair<string, int>>> Handle(GetChatMessageLeaderboardQuery request, CancellationToken cancellationToken)
    {
        var x = _context.ChatMessages
            .GroupBy(cm => cm.SenderUsername)
            .OrderByDescending(grp => grp.Count())
            .Select(grp => new KeyValuePair<string, int>(grp.Key, grp.Count()));

        return await Task.FromResult(x);
    }
}