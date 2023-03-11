using FirebotProxy.SecondaryPorts.RemoveMessagesByUsername;
using MediatR;

namespace FirebotProxy.Domain.Tests.FakeHandlers;

public class FakeRemoveMessagesByUsernameCommandHandler : IRequestHandler<RemoveMessagesByUsernameCommand>
{
    private readonly bool _shouldThrowException;

    public FakeRemoveMessagesByUsernameCommandHandler(bool shouldThrowException)
    {
        _shouldThrowException = shouldThrowException;
    }

    public async Task Handle(RemoveMessagesByUsernameCommand request, CancellationToken cancellationToken)
    {
        if (_shouldThrowException)
        {
            throw new Exception($"test exception message from {nameof(FakeRemoveMessagesByUsernameCommandHandler)}");
        }

        await Unit.Task;
    }
}