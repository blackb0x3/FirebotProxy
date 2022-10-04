using FirebotProxy.Domain.Representations;
using MediatR;
using OneOf;

namespace FirebotProxy.Domain.PrimaryPorts.GetViewerChatRank;

public class GetViewerChatRankRequest : IRequest<OneOf<GetViewerChatRankResponse, ErrorRepresentation>>
{
    public string ViewerUsername { get; set; } = null!;
}