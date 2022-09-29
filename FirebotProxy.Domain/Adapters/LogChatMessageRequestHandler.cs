using FirebotProxy.Data.Entities;
using FirebotProxy.Domain.PrimaryPorts.LogChatMessage;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Extensions;
using FirebotProxy.SecondaryPorts.SaveChatMessage;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace FirebotProxy.Domain.Adapters;

internal class LogChatMessageRequestHandler : IRequestHandler<LogChatMessageRequest, OneOf<LogChatMessageSuccess, ErrorRepresentation>>
{
    private readonly ILogger<LogChatMessageRequestHandler> _logger;
    private readonly IMediator _mediator;

    public LogChatMessageRequestHandler(ILogger<LogChatMessageRequestHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<OneOf<LogChatMessageSuccess, ErrorRepresentation>> Handle(LogChatMessageRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInfo(new { msg = "Handler called", request, handler = nameof(LogChatMessageRequestHandler) });

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
                handler = nameof(LogChatMessageRequestHandler),
                exception = e.Message,
                e.StackTrace
            });

            return new ErrorRepresentation { Message = msg };
        }
    }

    private async Task<LogChatMessageSuccess> HandleInternal(LogChatMessageRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInfo(new { msg = "Saving chat message to storage", request });

        var saveChatMessageRequest = new SaveChatMessageCommand
        {
            ChatMessage = new ChatMessage
            {
                Content = request.Content,
                SenderUsername = request.SenderUsername,
                Timestamp = request.Timestamp
            }
        };

        await _mediator.Send(saveChatMessageRequest, cancellationToken);

        _logger.LogInfo(new { msg = "Chat message saved", request });

        return new LogChatMessageSuccess();
    }
}