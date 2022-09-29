using FirebotProxy.Api.Models.Request;
using FirebotProxy.Domain.PrimaryPorts.RemoveMessagesOfBannedViewer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using LogChatMessageRequest = FirebotProxy.Api.Models.Request.LogChatMessageRequest;

namespace FirebotProxy.Api.Controllers;

public class EventsController : ProxyControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly IMediator _mediator;

    public EventsController(ILogger<EventsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("LogChatMessage")]
    public async Task<IResult> LogChatMessage([FromBody] LogChatMessageRequest request)
    {
        var command = new Domain.PrimaryPorts.LogChatMessage.LogChatMessageRequest
        {
            Content = request.Content,
            SenderUsername = request.SenderUsername,
            Timestamp = request.TimestampToDateTime
        };

        var resp = await _mediator.Send(command);

        return resp.Match(
            _ => Results.Ok("Chat message logged"),
            error => Results.Problem(error.Message, statusCode: 500)
        );
    }

    [HttpPost("RemoveBannedViewerMessages")]
    public async Task<IResult> RemoveBannedViewerMessages([FromBody] RemoveBannedViewerMessagesRequest request)
    {
        var command = new RemoveMessagesOfBannedViewerRequest
        {
            BannedViewerUsername = request.BannedViewerUsername
        };

        var resp = await _mediator.Send(command);

        return resp.Match(
            _ => Results.Ok("Viewer messages removed"),
            invalidRequest => Results.BadRequest(invalidRequest),
            error => Results.Problem(error.Message, statusCode: 500)
        );
    }
}