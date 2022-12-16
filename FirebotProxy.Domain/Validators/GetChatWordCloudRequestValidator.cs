using FirebotProxy.Domain.PrimaryPorts.GetChatWordCloud;
using FluentValidation;

namespace FirebotProxy.Domain.Validators;

public class GetChatWordCloudRequestValidator : AbstractValidator<GetChatWordCloudRequest>
{
    public GetChatWordCloudRequestValidator()
    {
        RuleFor(request => request.ViewerUsername)
            .Must(BeAValidUsername)
            .WithMessage($"{nameof(GetChatWordCloudRequest.ViewerUsername)} can be null, but must not be empty or made of pure whitespace.");

        RuleFor(request => request.StreamDate)
            .Must(BeAValidDate)
            .WithMessage($"{nameof(GetChatWordCloudRequest.StreamDate)} can be null, but must be a valid timestamp if supplied.");

        RuleFor(request => request.WordCloudSettings)
            .SetValidator(new WordCloudSettingsValidator())
            .WithMessage("Invalid word cloud settings detected.");
    }

    private static bool BeAValidUsername(string? usernameToCheck)
    {
        // no username is ok
        // we create a word cloud for the entire chat instead
        if (usernameToCheck is null)
        {
            return true;
        }

        // if not null, then username cannot be empty string or pure whitespace
        return !string.IsNullOrWhiteSpace(usernameToCheck);
    }

    private static bool BeAValidDate(string? dateStringToCheck)
    {
        // no stream date is ok
        // we create a word cloud using chat messages from all streams
        if (dateStringToCheck is null)
        {
            return true;
        }

        return DateTime.TryParse(dateStringToCheck, out _);
    }
}