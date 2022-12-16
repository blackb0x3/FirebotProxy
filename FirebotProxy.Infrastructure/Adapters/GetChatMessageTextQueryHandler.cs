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
        var chatMessages = _context.ChatMessages
            .AsNoTracking()
            .AsSplitQuery();

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

        // get the most common words across every message
        var wordCount = new Dictionary<string, uint>();
        var messages = await chatMessages.Select(msg => msg.Content).ToListAsync(cancellationToken);

        foreach (var word in messages.SelectMany(msg => msg.ReplaceLineEndings(" ").Split()))
        {
            wordCount.TryAdd(word, default);
            wordCount[word] += 1;
        }

        // pick the top 100
        var words = wordCount.OrderByDescending(kvp => kvp.Value).Take(request.NumberOfWordsToTake).Select(kvp => kvp.Key);

        // now join them up
        var chatText = string.Join(" ", words);

        return await Task.FromResult(chatText);
    }
}