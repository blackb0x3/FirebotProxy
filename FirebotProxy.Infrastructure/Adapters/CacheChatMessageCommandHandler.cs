using System.Globalization;
using System.Text;
using FirebotProxy.SecondaryPorts.CacheChatMessage;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FirebotProxy.Infrastructure.Adapters;

public class CacheChatMessageCommandHandler : IRequestHandler<CacheChatMessageCommand>
{
    private readonly ILogger<CacheChatMessageCommandHandler> _logger;
    private readonly IDistributedCache _cache;

    public CacheChatMessageCommandHandler(ILogger<CacheChatMessageCommandHandler> logger, IDistributedCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<Unit> Handle(CacheChatMessageCommand request, CancellationToken cancellationToken)
    {
        var key = ConstructCacheKey(request);
        var value = JsonConvert.SerializeObject(request.ChatMessage, Formatting.None);

        await _cache.SetStringAsync(key, value, cancellationToken);

        return Unit.Value;
    }

    private static string ConstructCacheKey(CacheChatMessageCommand request)
    {
        var sb = new StringBuilder("ChatMessage_");

        var components = new List<string>
        {
            request.ChatMessage.SenderUsername,
            request.ChatMessage.Timestamp.ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture)
        };

        sb.Append(string.Join('_', components));

        return sb.ToString();
    }
}