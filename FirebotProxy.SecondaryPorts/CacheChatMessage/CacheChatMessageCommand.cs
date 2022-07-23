using FirebotProxy.SecondaryPorts.Models;
using MediatR;

namespace FirebotProxy.SecondaryPorts.CacheChatMessage;

public class CacheChatMessageCommand : IRequest<Unit>
{
    public ChatMessage ChatMessage { get; set; }
}