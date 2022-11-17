using FirebotProxy.Domain.Representations;
using MediatR;
using OneOf;

namespace FirebotProxy.Domain.PrimaryPorts.RemoveViewerMessages;

public class RemoveViewerMessagesRequest : IRequest<OneOf<RemoveViewerMessagesSuccess, ValidationRepresentation, ErrorRepresentation>>
{
    public string ViewerUsername { get; set; } = null!;
}