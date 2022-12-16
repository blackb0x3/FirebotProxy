namespace FirebotProxy.Domain.Representations;

public class ValidationRepresentation
{
    public ValidationRepresentation(FluentValidation.Results.ValidationResult result)
    {
        Errors = result.Errors.Select(e => $"`{e.PropertyName}`: {e.ErrorMessage}").ToList();
    }

    public ValidationRepresentation(string errorMessage)
    {
        Errors = new List<string> { errorMessage };
    }

    public IReadOnlyCollection<string> Errors { get; set; }
}