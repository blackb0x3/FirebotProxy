using FirebotProxy.Data.Entities;
using FirebotProxy.Domain.PrimaryPorts.LogChatMessage;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Extensions;
using FirebotProxy.SecondaryPorts.SaveChatMessage;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace FirebotProxy.Domain.Adapters;

internal class LogChatMessageCommandHandler : IRequestHandler<LogChatMessageCommand, OneOf<LogChatMessageSuccess, ErrorRepresentation>>
{
    private readonly ILogger<LogChatMessageCommandHandler> _logger;
    private readonly IMediator _mediator;

    public LogChatMessageCommandHandler(ILogger<LogChatMessageCommandHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<OneOf<LogChatMessageSuccess, ErrorRepresentation>> Handle(LogChatMessageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo(new { msg = "Handler called", request, handler = nameof(LogChatMessageCommandHandler) });

        try
        {
            return await HandleInternal(request, cancellationToken);
        }
        catch (Exception e)
        {
            const string msg = "Failed to log chat message";

            _logger.LogError(new
            {
                msg,
                request,
                handler = nameof(LogChatMessageCommandHandler),
                exception = e.Message,
                e.StackTrace
            });

            return new ErrorRepresentation { Message = msg };
        }
    }

    private async Task<LogChatMessageSuccess> HandleInternal(LogChatMessageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug(new { msg = "Saving chat message to storage", request });

        var saveChatMessageCommand = new SaveChatMessageCommand
        {
            ChatMessage = new ChatMessage
            {
                Content = request.Content,
                SenderUsername = request.SenderUsername,
                Timestamp = request.Timestamp
            }
        };

        await _mediator.Send(saveChatMessageCommand, cancellationToken);

        _logger.LogDebug(new { msg = "Chat message saved", request });

        return new LogChatMessageSuccess();
    }
}