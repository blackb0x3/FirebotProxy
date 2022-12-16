using FluentValidation;
using FluentValidation.Results;

namespace FirebotProxy.TestBase;

public class FakeValidator<T> : AbstractValidator<T>
{
    private readonly bool _shouldValidateRequest;

    public FakeValidator(bool shouldValidateRequest)
    {
        _shouldValidateRequest = shouldValidateRequest;
    }

    public override ValidationResult Validate(ValidationContext<T> context)
    {
        return GenerateMockValidationResult();
    }

    public override async Task<ValidationResult> ValidateAsync(ValidationContext<T> context, CancellationToken cancellation = new())
    {
        return await Task.FromResult(GenerateMockValidationResult());
    }

    private ValidationResult GenerateMockValidationResult()
    {
        var validationResult = new ValidationResult();

        if (!_shouldValidateRequest)
        {
            validationResult.Errors.Add(new ValidationFailure());
        }

        return validationResult;
    }
}