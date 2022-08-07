using FirebotProxy.Api.Models.Request;
using FirebotProxy.Domain.PrimaryPorts.LogChatMessage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        var command = new LogChatMessageCommand
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
}