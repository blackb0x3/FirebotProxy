namespace FirebotProxy.Domain.Representations;

public class ValidationRepresentation
{
    public ValidationRepresentation(FluentValidation.Results.ValidationResult result)
    {
        Errors = result.Errors.Select(e => $"{e.PropertyName} : {e.ErrorMessage}").ToArray();
    }

    public IReadOnlyCollection<string> Errors { get; set; }
}