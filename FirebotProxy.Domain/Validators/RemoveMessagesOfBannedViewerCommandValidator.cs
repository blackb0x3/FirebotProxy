using FirebotProxy.Domain.PrimaryPorts.RemoveMessagesOfBannedViewer;
using FluentValidation;

namespace FirebotProxy.Domain.Validators;

public class RemoveMessagesOfBannedViewerRequestValidator : AbstractValidator<RemoveMessagesOfBannedViewerRequest>
{
    public RemoveMessagesOfBannedViewerRequestValidator()
    {
        RuleFor(req => req.BannedViewerUsername)
            .NotNull()
            .NotEmpty()
            .WithMessage("Viewer username must not be null or empty.");
    }
}