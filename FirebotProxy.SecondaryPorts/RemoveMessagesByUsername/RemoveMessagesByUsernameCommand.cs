using MediatR;

namespace FirebotProxy.SecondaryPorts.RemoveMessagesByUsername;

public class RemoveMessagesByUsernameCommand : IRequest
{
    public string SenderUsername { get; set; } = null!;
}