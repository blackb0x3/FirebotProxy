using FirebotProxy.Domain.Representations;
using MediatR;
using OneOf;

namespace FirebotProxy.Domain.PrimaryPorts.RemoveMessagesOfBannedViewer;

public class RemoveMessagesOfBannedViewerCommand : IRequest<OneOf<RemoveMessagesOfBannedViewerSuccess, ValidationRepresentation, ErrorRepresentation>>
{
    public string BannedViewerUsername { get; set; }
}