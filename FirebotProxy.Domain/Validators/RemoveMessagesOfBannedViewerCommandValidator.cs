using FirebotProxy.Domain.PrimaryPorts.RemoveMessagesOfBannedViewer;
using FirebotProxy.Validation;
using FluentValidation;

namespace FirebotProxy.Domain.Validators;

public class RemoveMessagesOfBannedViewerRequestValidator : AbstractValidator<RemoveMessagesOfBannedViewerRequest>
{
    public RemoveMessagesOfBannedViewerRequestValidator()
    {
        RuleFor(req => req.BannedViewerUsername)
            .SetValidator(new IsNullEmptyOrWhitespacePropertyValidator<RemoveMessagesOfBannedViewerRequest>())
            .WithMessage("Viewer username must not be null or empty.");
    }
}