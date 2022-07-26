﻿using FirebotProxy.Api.Models.Request;
using FirebotProxy.Domain.PrimaryPorts.GetChatWordCloud;
using FirebotProxy.Domain.PrimaryPorts.GetViewerChatPlot;
using FirebotProxy.Domain.PrimaryPorts.GetViewerChatRank;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FirebotProxy.Api.Controllers;

public class CommandsController : ProxyControllerBase
{
    private readonly ILogger<CommandsController> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CommandsController(ILogger<CommandsController> logger, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("ChatRank/{viewerUsername}")]
    public async Task<IResult> ChatRank(string viewerUsername)
    {
        var request = new GetViewerChatRankRequest { ViewerUsername = viewerUsername };

        var response = await _mediator.Send(request);

        return response.Match(
            result => Results.Ok(result),
            error => Results.Problem(error.Message, statusCode: 500)
        );
    }

    [HttpGet("ChatPlot/{viewerUsername}")]
    public async Task<IResult> ChatPlot(string viewerUsername)
    {
        if (!Request.Query.TryGetValue("chartType", out var chartType))
        {
            chartType = "Line";
        }

        var request = new GetViewerChatPlotRequest { ViewerUsername = viewerUsername, ChartType = chartType };

        var response = await _mediator.Send(request);

        return response.Match(
            result => Results.Ok(result),
            validation => Results.BadRequest(validation),
            error => Results.Problem(error.Message, statusCode: 500)
        );
    }

    [HttpPost("WordCloud")]
    public async Task<IResult> WordCloud([FromBody] GetChatWordCloudRequestModel model)
    {
        var request = _mapper.Map<GetChatWordCloudRequest>(model);
        var response = await _mediator.Send(request);

        return response.Match(
            result => Results.Ok(result),
            validation => Results.BadRequest(validation),
            error => Results.Problem(error.Message, statusCode: 500)
        );
    }
}