using FirebotProxy.SecondaryPorts.CacheChatMessage;
using MediatR;

namespace FirebotProxy.Infrastructure.Adapters;

public class CacheChatMessageCommandHandler : IRequestHandler<CacheChatMessageCommand>
{
    public async Task<Unit> Handle(CacheChatMessageCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}