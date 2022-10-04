using FirebotProxy.Data.Entities;
using MediatR;

namespace FirebotProxy.SecondaryPorts.SaveChatMessage;

public class SaveChatMessageCommand : IRequest<Unit>
{
    public ChatMessage ChatMessage { get; set; } = null!;
}