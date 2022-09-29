using FluentValidation;
using FluentValidation.Validators;

namespace FirebotProxy.Validation;

public class IsNullEmptyOrWhitespacePropertyValidator<TToValidate> : PropertyValidator<TToValidate, string>
{
    public override bool IsValid(ValidationContext<TToValidate> context, string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    public override string Name => nameof(IsNullEmptyOrWhitespacePropertyValidator<TToValidate>);
}