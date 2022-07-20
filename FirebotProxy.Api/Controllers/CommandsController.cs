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
            result => Results.Ok(result.ChatRankUrl),
            failure => Results.Problem(failure.Message, statusCode: 500)
        );
    }
}