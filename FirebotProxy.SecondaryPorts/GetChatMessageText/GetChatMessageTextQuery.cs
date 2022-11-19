using MediatR;

namespace FirebotProxy.SecondaryPorts.GetChatMessageText;

public class GetChatMessageTextQuery : IRequest<string>
{
    public string? ViewerUsername { get; set; }

    public DateTime? StreamDate { get; set; }
}