using FluentValidation;
using FluentValidation.Validators;

namespace FirebotProxy.Validation;

public class IsEnumMemberPropertyValidator<TToValidate, TEnum> : PropertyValidator<TToValidate, string> where TEnum : struct
{
    public override bool IsValid(ValidationContext<TToValidate> context, string value)
    {
        return Enum.TryParse<TEnum>(value, true, out _);
    }

    public override string Name => nameof(IsEnumMemberPropertyValidator<TToValidate, TEnum>);
}