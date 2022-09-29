using FirebotProxy.Domain.PrimaryPorts.LogChatMessage;
using FirebotProxy.Validation;
using FluentValidation;

namespace FirebotProxy.Domain.Validators;

public class LogChatMessageRequestValidator : AbstractValidator<LogChatMessageRequest>
{
    public LogChatMessageRequestValidator()
    {
        RuleFor(request => request.Content)
            .SetValidator(new IsNullEmptyOrWhitespacePropertyValidator<LogChatMessageRequest>())
            .WithMessage("Message content must not be null or empty.");

        RuleFor(request => request.SenderUsername)
            .SetValidator(new IsNullEmptyOrWhitespacePropertyValidator<LogChatMessageRequest>())
            .WithMessage("Sender username must not be null or empty.");

        RuleFor(request => request.Timestamp)
            .NotNull();
    }
}