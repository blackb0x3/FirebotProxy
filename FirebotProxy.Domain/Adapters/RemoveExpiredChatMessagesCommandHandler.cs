using FirebotProxy.Domain.PrimaryPorts.RemoveExpiredChatMessages;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace FirebotProxy.Domain.Adapters;

internal class RemoveExpiredChatMessagesCommandHandler : IRequestHandler<PrimaryPorts.RemoveExpiredChatMessages.RemoveExpiredChatMessagesCommand, OneOf<RemoveExpiredChatMessagesSuccess, ErrorRepresentation>>
{
    private const int CutOffDays = 30;

    private readonly ILogger<RemoveExpiredChatMessagesCommandHandler> _logger;
    private readonly IMediator _mediator;

    public RemoveExpiredChatMessagesCommandHandler(ILogger<RemoveExpiredChatMessagesCommandHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<OneOf<RemoveExpiredChatMessagesSuccess, ErrorRepresentation>> Handle(PrimaryPorts.RemoveExpiredChatMessages.RemoveExpiredChatMessagesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo(new { msg = "Handler called", request, handler = nameof(RemoveExpiredChatMessagesCommandHandler) });

        try
        {
            return await HandleInternal(cancellationToken);
        }
        catch (Exception e)
        {
            const string msg = "Failed to remove expired chat messages";

            _logger.LogError(new
            {
                msg,
                request,
                handler = nameof(RemoveExpiredChatMessagesCommandHandler),
                exception = e.Message,
                e.StackTrace
            });

            return new ErrorRepresentation { Message = msg };
        }
    }

    private async Task<RemoveExpiredChatMessagesSuccess> HandleInternal(CancellationToken cancellationToken)
    {
        _logger.LogInfo(new { msg = "Removing expired chat messages", CutOffDays });

        var removeExpiredChatMessagesCommand = new SecondaryPorts.RemoveExpiredChatMessages.RemoveExpiredChatMessagesCommand { Cutoff = DateTime.UtcNow.AddDays(-CutOffDays) };

        await _mediator.Send(removeExpiredChatMessagesCommand, cancellationToken);

        _logger.LogInfo(new { msg = "Expired chat messages were removed", CutOffDays });

        return new RemoveExpiredChatMessagesSuccess();
    }
}