﻿using System.Collections.Specialized;
using FirebotProxy.Domain.PrimaryPorts.GetViewerChatRank;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Helpers;
using FirebotProxy.SecondaryPorts.GetAllChatMessages;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

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
        try
        {
            var allMessages = await _mediator.Send(new GetAllChatMessagesQuery(), cancellationToken);
            var viewerMsgCount = allMessages.Count(cm => string.Equals(cm.SenderUsername, request.ViewerUsername));
            var userMsgCountLookup = allMessages.GroupBy(cm => cm.SenderUsername)
                .ToDictionary(grp => grp.Key, grp => grp.Count())
                .OrderByDescending(x => x.Value)
                .Select(x => x.Key)
                .ToList();

            var viewerRankPosition = userMsgCountLookup.IndexOf(request.ViewerUsername) + 1;

            return new GetViewerChatRankResponse
            {
                MessageCount = viewerMsgCount,
                RankPosition = RankPositionHelper.GetRankPositionFromInteger(viewerRankPosition)
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogError(e.StackTrace);

            return new ErrorRepresentation { Message = e.Message };
        }
    }
}