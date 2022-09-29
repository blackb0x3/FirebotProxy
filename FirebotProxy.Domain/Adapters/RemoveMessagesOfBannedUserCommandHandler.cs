using FirebotProxy.Domain.PrimaryPorts.RemoveMessagesOfBannedViewer;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Extensions;
using FirebotProxy.SecondaryPorts.RemoveMessagesByUsername;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace FirebotProxy.Domain.Adapters;

internal class RemoveMessagesOfBannedViewerRequestHandler : IRequestHandler<RemoveMessagesOfBannedViewerRequest,
    OneOf<RemoveMessagesOfBannedViewerSuccess, ValidationRepresentation, ErrorRepresentation>>
{
    private readonly ILogger<RemoveMessagesOfBannedViewerRequestHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IValidator<RemoveMessagesOfBannedViewerRequest> _validator;

    public RemoveMessagesOfBannedViewerRequestHandler(ILogger<RemoveMessagesOfBannedViewerRequestHandler> logger,
        IMediator mediator, IValidator<RemoveMessagesOfBannedViewerRequest> validator)
    {
        _logger = logger;
        _mediator = mediator;
        _validator = validator;
    }

    public async Task<OneOf<RemoveMessagesOfBannedViewerSuccess, ValidationRepresentation, ErrorRepresentation>> Handle(
        RemoveMessagesOfBannedViewerRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInfo(new { msg = "Handler called", request, handler = nameof(RemoveMessagesOfBannedViewerRequestHandler) });

        try
        {
            var result = await HandleInternal(request, cancellationToken);

            return result.Match<OneOf<RemoveMessagesOfBannedViewerSuccess, ValidationRepresentation, ErrorRepresentation>>(
                success => success,
                validationResult => validationResult
            );
        }
        catch (Exception e)
        {
            const string msg = "Failed to remove messages of banned viewer";

            _logger.LogError(new
            {
                msg,
                request,
                handler = nameof(RemoveMessagesOfBannedViewerRequestHandler),
                exception = e.Message,
                e.StackTrace
            });

            return new ErrorRepresentation { Message = msg };
        }
    }

    private async Task<OneOf<RemoveMessagesOfBannedViewerSuccess, ValidationRepresentation>> HandleInternal(RemoveMessagesOfBannedViewerRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInfo(new { msg = "Validating request", request, requestType = nameof(RemoveMessagesOfBannedViewerRequest) });

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(new { msg = "Request determined to be invalid", request, requestType = nameof(RemoveMessagesOfBannedViewerRequest), validationResult });
            return new ValidationRepresentation(validationResult);
        }

        _logger.LogInfo(new { msg = "Removing chat messages via secondary port", request });
        var removeMessagesByUsernameRequest = new RemoveMessagesByUsernameCommand { SenderUsername = request.BannedViewerUsername };

        await _mediator.Send(removeMessagesByUsernameRequest, cancellationToken);

        _logger.LogInfo(new { msg = "Chat messages removed", request });
        return new RemoveMessagesOfBannedViewerSuccess();
    }
}