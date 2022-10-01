using FirebotProxy.SecondaryPorts.RemoveExpiredChatMessages;
using MediatR;

namespace FirebotProxy.Domain.Tests.FakeHandlers;

public class FakeRemoveExpiredChatMessagesCommandHandler : IRequestHandler<RemoveExpiredChatMessagesCommand>
{
    private readonly bool _shouldThrowException;

    public FakeRemoveExpiredChatMessagesCommandHandler(bool shouldThrowException)
    {
        _shouldThrowException = shouldThrowException;
    }

    public async Task<Unit> Handle(RemoveExpiredChatMessagesCommand request, CancellationToken cancellationToken)
    {
        if (_shouldThrowException)
        {
            throw new Exception($"test exception message from {nameof(FakeRemoveMessagesByUsernameCommandHandler)}");
        }

        return await Unit.Task;
    }
}