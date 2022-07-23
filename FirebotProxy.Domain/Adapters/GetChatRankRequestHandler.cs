﻿using FirebotProxy.Domain.PrimaryPorts.GetViewerChatRank;
using FirebotProxy.Domain.Representations;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace FirebotProxy.Domain.Adapters;

internal class GetViewerChatRankRequestHandler : IRequestHandler<GetViewerChatRankRequest, OneOf<GetViewerChatRankResponse, ErrorRepresentation>>
{
    private readonly ILogger<GetViewerChatRankRequestHandler> _logger;

    public GetViewerChatRankRequestHandler(ILogger<GetViewerChatRankRequestHandler> logger)
    {
        _logger = logger;
    }

    public async Task<OneOf<GetViewerChatRankResponse, ErrorRepresentation>> Handle(GetViewerChatRankRequest request, CancellationToken cancellationToken)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogError(e.StackTrace);

            return new ErrorRepresentation { Message = e.Message };
        }
    }
}