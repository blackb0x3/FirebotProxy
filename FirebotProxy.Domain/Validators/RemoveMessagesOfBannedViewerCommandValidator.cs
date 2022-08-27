using FirebotProxy.Domain.PrimaryPorts.RemoveMessagesOfBannedViewer;
using FluentValidation;

namespace FirebotProxy.Domain.Validators;

public class RemoveMessagesOfBannedViewerCommandValidator : AbstractValidator<RemoveMessagesOfBannedViewerCommand>
{
    public RemoveMessagesOfBannedViewerCommandValidator()
    {
        RuleFor(req => req.BannedViewerUsername)
            .NotNull()
            .NotEmpty()
            .WithMessage("Viewer username must not be null or empty.");
    }
}