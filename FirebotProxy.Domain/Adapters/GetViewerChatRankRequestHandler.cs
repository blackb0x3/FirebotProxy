using System.Runtime.CompilerServices;
using FirebotProxy.Domain.PrimaryPorts.GetViewerChatRank;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Extensions;
using FirebotProxy.Helpers;
using FirebotProxy.SecondaryPorts.GetChatMessageLeaderboard;
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
        _logger.LogInfo(new { msg = "Retrieving chat message leaderboard from storage", request });
        var leaderboard = await _mediator.Send(new GetChatMessageLeaderboardQuery(), cancellationToken);

        _logger.LogInfo(new { msg = "Looking up viewer in the leaderboard" });

        // assume first place to start with
        var position = 1;

        foreach (var entry in leaderboard)
        {
            if (string.Equals(request.ViewerUsername, entry.Key, StringComparison.OrdinalIgnoreCase))
            {
                return new GetViewerChatRankResponse
                {
                    MessageCount = entry.Value,
                    RankPosition = RankPositionHelper.GetRankPositionFromInteger(position)
                };
            }

            position++;
        }

        return new GetViewerChatRankResponse
        {
            MessageCount = 0,
            RankPosition = RankPositionHelper.GetRankPositionFromInteger(position)
        };
    }
}