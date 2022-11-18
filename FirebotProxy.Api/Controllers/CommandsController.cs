using FirebotProxy.Domain.PrimaryPorts.GetViewerChatPlot;
using FirebotProxy.Domain.PrimaryPorts.GetViewerChatRank;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FirebotProxy.Api.Controllers;

public class CommandsController : ProxyControllerBase
{
    private readonly ILogger<CommandsController> _logger;
    private readonly IMediator _mediator;

    public CommandsController(ILogger<CommandsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
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

    [HttpGet("WordCloud")]
    public async Task<IResult> WordCloud()
    {
        var request = someMapper.MapStuff();
        var response = await _mediator.Send(request);

        return response.Match(
            result => Results.Ok(result),
            validation => Results.BadRequest(validation),
            error => Results.Problem(error.Message, statusCode: 500)
        );
    }
}