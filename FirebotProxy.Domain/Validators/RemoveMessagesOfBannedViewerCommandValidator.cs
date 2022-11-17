using FirebotProxy.Domain.PrimaryPorts.RemoveViewerMessages;
using FirebotProxy.Validation;
using FluentValidation;

namespace FirebotProxy.Domain.Validators;

public class RemoveMessagesOfBannedViewerRequestValidator : AbstractValidator<RemoveViewerMessagesRequest>
{
    public RemoveMessagesOfBannedViewerRequestValidator()
    {
        RuleFor(req => req.ViewerUsername)
            .SetValidator(new IsNullEmptyOrWhitespacePropertyValidator<RemoveViewerMessagesRequest>())
            .WithMessage("Viewer username must not be null or empty.");
    }
}