using FirebotProxy.Domain.Representations;
using MediatR;
using OneOf;

namespace FirebotProxy.Domain.PrimaryPorts.GetViewerChatPlot;

public class GetViewerChatPlotRequest : IRequest<OneOf<GetViewerChatPlotResponse, ValidationRepresentation, ErrorRepresentation>>
{
    public string ViewerUsername { get; set; } = null!;

    public string ChartType { get; set; } = null!;
}