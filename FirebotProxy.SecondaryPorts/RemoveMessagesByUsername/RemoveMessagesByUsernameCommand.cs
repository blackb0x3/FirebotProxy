using MediatR;

namespace FirebotProxy.SecondaryPorts.RemoveMessagesByUsername;

public class RemoveMessagesByUsernameCommand : IRequest<Unit>
{
    public string SenderUsername { get; set; }
}