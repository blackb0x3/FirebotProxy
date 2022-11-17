using FirebotProxy.Domain.PrimaryPorts.RemoveViewerMessages;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Extensions;
using FirebotProxy.SecondaryPorts.RemoveMessagesByUsername;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace FirebotProxy.Domain.Adapters;

internal class RemoveViewerMessagesRequestHandler : IRequestHandler<RemoveViewerMessagesRequest,
    OneOf<RemoveViewerMessagesSuccess, ValidationRepresentation, ErrorRepresentation>>
{
    private readonly ILogger<RemoveViewerMessagesRequestHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IValidator<RemoveViewerMessagesRequest> _validator;

    public RemoveViewerMessagesRequestHandler(ILogger<RemoveViewerMessagesRequestHandler> logger,
        IMediator mediator, IValidator<RemoveViewerMessagesRequest> validator)
    {
        _logger = logger;
        _mediator = mediator;
        _validator = validator;
    }

    public async Task<OneOf<RemoveViewerMessagesSuccess, ValidationRepresentation, ErrorRepresentation>> Handle(
        RemoveViewerMessagesRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInfo(new { msg = "Handler called", request, handler = nameof(RemoveViewerMessagesRequestHandler) });

        try
        {
            var result = await HandleInternal(request, cancellationToken);

            return result.Match<OneOf<RemoveViewerMessagesSuccess, ValidationRepresentation, ErrorRepresentation>>(
                success => success,
                validationResult => validationResult
            );
        }
        catch (Exception e)
        {
            const string msg = "Failed to remove messages of viewer";

            _logger.LogError(new
            {
                msg,
                request,
                handler = nameof(RemoveViewerMessagesRequestHandler),
                exception = e.Message,
                e.StackTrace
            });

            return new ErrorRepresentation { Message = msg };
        }
    }

    private async Task<OneOf<RemoveViewerMessagesSuccess, ValidationRepresentation>> HandleInternal(RemoveViewerMessagesRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInfo(new { msg = "Validating request", request, requestType = nameof(RemoveViewerMessagesRequest) });

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(new { msg = "Request determined to be invalid", request, requestType = nameof(RemoveViewerMessagesRequest), validationResult });
            return new ValidationRepresentation(validationResult);
        }

        _logger.LogInfo(new { msg = "Removing chat messages via secondary port", request });
        var removeMessagesByUsernameRequest = new RemoveMessagesByUsernameCommand { SenderUsername = request.ViewerUsername };

        await _mediator.Send(removeMessagesByUsernameRequest, cancellationToken);

        _logger.LogInfo(new { msg = "Chat messages removed", request });
        return new RemoveViewerMessagesSuccess();
    }
}