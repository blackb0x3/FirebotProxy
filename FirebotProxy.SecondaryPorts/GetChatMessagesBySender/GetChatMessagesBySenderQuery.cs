using FirebotProxy.Data.Entities;
using MediatR;

namespace FirebotProxy.SecondaryPorts.GetChatMessages;

public class GetChatMessagesBySenderQuery : IRequest<IReadOnlyCollection<ChatMessage>>
{
    public string SenderUsername { get; set; }
}