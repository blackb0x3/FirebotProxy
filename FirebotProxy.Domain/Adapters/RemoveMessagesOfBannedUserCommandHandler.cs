﻿using FirebotProxy.Domain.PrimaryPorts.RemoveMessagesOfBannedViewer;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Extensions;
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
        _logger.LogInfo(new { msg = "Handler called", request, handler = nameof(RemoveMessagesOfBannedViewerCommandHandler) });

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
                handler = nameof(RemoveMessagesOfBannedViewerCommandHandler),
                exception = e.Message,
                e.StackTrace
            });

            return new ErrorRepresentation { Message = msg };
        }
    }

    private async Task<OneOf<RemoveMessagesOfBannedViewerSuccess, ValidationRepresentation>> HandleInternal(RemoveMessagesOfBannedViewerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo(new { msg = "Validating request", request, requestType = nameof(RemoveMessagesOfBannedViewerCommand) });

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(new { msg = "Request determined to be invalid", request, requestType = nameof(RemoveMessagesOfBannedViewerCommand), validationResult });
            return new ValidationRepresentation(validationResult);
        }

        _logger.LogInfo(new { msg = "Removing chat messages via secondary port", request });
        var removeMessagesByUsernameCommand = new RemoveMessagesByUsernameCommand { SenderUsername = request.BannedViewerUsername };

        await _mediator.Send(removeMessagesByUsernameCommand, cancellationToken);

        _logger.LogInfo(new { msg = "Chat messages removed", request });
        return new RemoveMessagesOfBannedViewerSuccess();
    }
}