using FirebotProxy.Domain.PrimaryPorts.RemoveExpiredChatMessages;
using FirebotProxy.Domain.Representations;
using FirebotProxy.SecondaryPorts.RemoveChatMessages;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace FirebotProxy.Domain.Adapters;

internal class RemoveExpiredChatMessagesCommandHandler : IRequestHandler<RemoveExpiredChatMessagesCommand, OneOf<RemoveExpiredChatMessagesSuccess, ErrorRepresentation>>
{
    private readonly ILogger<RemoveExpiredChatMessagesCommandHandler> _logger;
    private readonly IMediator _mediator;

    public RemoveExpiredChatMessagesCommandHandler(ILogger<RemoveExpiredChatMessagesCommandHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<OneOf<RemoveExpiredChatMessagesSuccess, ErrorRepresentation>> Handle(RemoveExpiredChatMessagesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var removeChatMessagesCommand = new RemoveChatMessagesCommand { Cutoff = DateTime.UtcNow.AddDays(-30) };

            var result = await _mediator.Send(removeChatMessagesCommand, cancellationToken);

            return new RemoveExpiredChatMessagesSuccess { MessagesRemoved = result.MessagesRemoved };
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogError(e.StackTrace);

            return new ErrorRepresentation { Message = e.Message };
        }
    }
}