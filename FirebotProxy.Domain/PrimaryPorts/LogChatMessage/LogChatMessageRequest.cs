using FirebotProxy.Domain.Representations;
using MediatR;
using OneOf;

namespace FirebotProxy.Domain.PrimaryPorts.LogChatMessage;

public class LogChatMessageRequest : IRequest<OneOf<LogChatMessageSuccess, ValidationRepresentation, ErrorRepresentation>>
{
    public string Content { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public string SenderUsername { get; set; } = null!;
}