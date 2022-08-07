using FirebotProxy.Data.Entities;
using FirebotProxy.Domain.PrimaryPorts.LogChatMessage;
using FirebotProxy.Domain.Representations;
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
        try
        {
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

            return new LogChatMessageSuccess();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogError(e.StackTrace);

            return new ErrorRepresentation { Message = e.Message };
        }
    }
}