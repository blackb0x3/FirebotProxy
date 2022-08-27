using FirebotProxy.Domain.InternalModels.GetViewerChatPlot;
using FirebotProxy.Domain.PrimaryPorts.GetViewerChatPlot;
using FirebotProxy.Validation;
using FluentValidation;

namespace FirebotProxy.Domain.Validators;

public class GetViewerChatPlotRequestValidator : AbstractValidator<GetViewerChatPlotRequest>
{
    public GetViewerChatPlotRequestValidator()
    {
        RuleFor(req => req.ViewerUsername)
            .NotNull()
            .NotEmpty()
            .WithMessage("Viewer username must not be null or empty.");

        RuleFor(req => req.ChartType)
            .SetValidator(new IsEnumMemberPropertyValidator<GetViewerChatPlotRequest, ChartType>())
            .WithMessage($"Unsupported chart type. Valid chart types are: {string.Join(", ", Enum.GetValues<ChartType>()).ToLower()}");
    }
}