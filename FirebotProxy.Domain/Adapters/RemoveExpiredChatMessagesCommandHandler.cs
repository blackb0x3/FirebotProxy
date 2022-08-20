using FirebotProxy.Domain.PrimaryPorts.RemoveExpiredChatMessages;
using FirebotProxy.Domain.Representations;
using FirebotProxy.SecondaryPorts.RemoveExpiredChatMessages;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace FirebotProxy.Domain.Adapters;

internal class RemoveExpiredChatMessagesCommandHandler : IRequestHandler<PrimaryPorts.RemoveExpiredChatMessages.RemoveExpiredChatMessagesCommand, OneOf<RemoveExpiredChatMessagesSuccess, ErrorRepresentation>>
{
    private readonly ILogger<RemoveExpiredChatMessagesCommandHandler> _logger;
    private readonly IMediator _mediator;

    public RemoveExpiredChatMessagesCommandHandler(ILogger<RemoveExpiredChatMessagesCommandHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<OneOf<RemoveExpiredChatMessagesSuccess, ErrorRepresentation>> Handle(PrimaryPorts.RemoveExpiredChatMessages.RemoveExpiredChatMessagesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var RemoveExpiredChatMessagesCommand = new SecondaryPorts.RemoveExpiredChatMessages.RemoveExpiredChatMessagesCommand { Cutoff = DateTime.UtcNow.AddDays(-30) };

            var result = await _mediator.Send(RemoveExpiredChatMessagesCommand, cancellationToken);

            return new RemoveExpiredChatMessagesSuccess();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogError(e.StackTrace);

            return new ErrorRepresentation { Message = e.Message };
        }
    }
}