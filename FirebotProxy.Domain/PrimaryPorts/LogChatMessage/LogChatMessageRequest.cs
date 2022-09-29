using FirebotProxy.Domain.Representations;
using MediatR;
using OneOf;

namespace FirebotProxy.Domain.PrimaryPorts.LogChatMessage;

public class LogChatMessageRequest : IRequest<OneOf<LogChatMessageSuccess, ErrorRepresentation>>
{
    public string Content { get; set; }

    public DateTime Timestamp { get; set; }

    public string SenderUsername { get; set; }
}