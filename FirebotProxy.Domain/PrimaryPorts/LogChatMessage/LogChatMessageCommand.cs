using FirebotProxy.Domain.Representations;
using MediatR;
using OneOf;

namespace FirebotProxy.Domain.PrimaryPorts.LogChatMessage;

public class LogChatMessageCommand : IRequest<OneOf<LogChatMessageSuccess, ErrorRepresentation>>
{
    public string Content { get; set; }

    public DateTime Timestamp { get; set; }

    public string SenderUsername { get; set; }
}