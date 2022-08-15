using FirebotProxy.Domain.PrimaryPorts.RemoveMessagesOfBannedViewer;
using FirebotProxy.Domain.Representations;
using FirebotProxy.SecondaryPorts.RemoveMessagesByUsername;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace FirebotProxy.Domain.Adapters;

internal class RemoveMessagesOfBannedViewerCommandHandler : IRequestHandler<RemoveMessagesOfBannedViewerCommand,
    OneOf<RemoveMessagesOfBannedViewerSuccess, ValidationRepresentation, ErrorRepresentation>>
{
    private readonly ILogger<RemoveMessagesOfBannedViewerCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IValidator<RemoveMessagesOfBannedViewerCommand> _validator;

    public RemoveMessagesOfBannedViewerCommandHandler(ILogger<RemoveMessagesOfBannedViewerCommandHandler> logger,
        IMediator mediator, IValidator<RemoveMessagesOfBannedViewerCommand> validator)
    {
        _logger = logger;
        _mediator = mediator;
        _validator = validator;
    }

    public async Task<OneOf<RemoveMessagesOfBannedViewerSuccess, ValidationRepresentation, ErrorRepresentation>> Handle(
        RemoveMessagesOfBannedViewerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new ValidationRepresentation(validationResult);
            }

            var removeMessagesByUsernameCommand = new RemoveMessagesByUsernameCommand { SenderUsername = request.BannedViewerUsername };

            await _mediator.Send(removeMessagesByUsernameCommand, cancellationToken);

            return new RemoveMessagesOfBannedViewerSuccess();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogError(e.StackTrace);

            return new ErrorRepresentation();
        }
    }
}