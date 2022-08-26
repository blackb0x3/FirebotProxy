using System.Runtime.CompilerServices;
using FirebotProxy.Domain.PrimaryPorts.GetViewerChatRank;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Extensions;
using FirebotProxy.Helpers;
using FirebotProxy.SecondaryPorts.GetAllChatMessages;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

[assembly: InternalsVisibleTo("FirebotProxy.Domain.Tests")]

namespace FirebotProxy.Domain.Adapters;

internal class GetViewerChatRankRequestHandler : IRequestHandler<GetViewerChatRankRequest, OneOf<GetViewerChatRankResponse, ErrorRepresentation>>
{
    private readonly ILogger<GetViewerChatRankRequestHandler> _logger;
    private readonly IMediator _mediator;

    public GetViewerChatRankRequestHandler(ILogger<GetViewerChatRankRequestHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<OneOf<GetViewerChatRankResponse, ErrorRepresentation>> Handle(GetViewerChatRankRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInfo(new { msg = "Handler called", request, handler = nameof(GetViewerChatRankRequestHandler) });

        try
        {
            return await HandleInternal(request, cancellationToken);
        }
        catch (Exception e)
        {
            const string msg = "Failed to get chat rank for viewer";

            _logger.LogError(new
            {
                msg,
                request,
                handler = nameof(GetViewerChatRankRequestHandler),
                exception = e.Message,
                e.StackTrace
            });

            return new ErrorRepresentation { Message = msg };
        }
    }

    private async Task<GetViewerChatRankResponse> HandleInternal(GetViewerChatRankRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug(new { msg = "Retrieving ALL chat messages from storage", request });
        var allMessages = await _mediator.Send(new GetAllChatMessagesQuery(), cancellationToken);

        _logger.LogDebug(new { msg = "Counting number of viewer messages", request });
        var viewerMsgCount = allMessages.Count(cm => string.Equals(cm.SenderUsername, request.ViewerUsername));

        _logger.LogDebug(new { msg = "Make a leaderboard and rank by message count", request });
        var userMsgCountLookup = allMessages.GroupBy(cm => cm.SenderUsername)
            .ToDictionary(grp => grp.Key, grp => grp.Count())
            .OrderByDescending(x => x.Value)
            .Select(x => x.Key)
            .ToList();

        _logger.LogDebug(new { msg = "Determining viewer position", request });
        var viewerRankPosition = userMsgCountLookup.IndexOf(request.ViewerUsername) + 1;

        return new GetViewerChatRankResponse
        {
            MessageCount = viewerMsgCount,
            RankPosition = RankPositionHelper.GetRankPositionFromInteger(viewerRankPosition)
        };
    }
}