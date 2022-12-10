using FirebotProxy.SecondaryPorts.GenerateWordCloud;
using MediatR;
using OneOf;

namespace FirebotProxy.Domain.Tests.FakeHandlers;

public class FakeGenerateWordCloudCommandHandler : IRequestHandler<GenerateWordCloudCommand, OneOf<GenerateWordCloudSuccess, GenerateWordCloudFailure>>
{
    private readonly bool _shouldReturnFailure;

    public FakeGenerateWordCloudCommandHandler(bool shouldReturnFailure)
    {
        _shouldReturnFailure = shouldReturnFailure;
    }

    public async Task<OneOf<GenerateWordCloudSuccess, GenerateWordCloudFailure>> Handle(GenerateWordCloudCommand request, CancellationToken cancellationToken)
    {
        if (_shouldReturnFailure)
        {
            return await Task.FromResult(new GenerateWordCloudFailure());
        }

        return await Task.FromResult(new GenerateWordCloudSuccess { WordCloudContent = "<svg></svg>" });
    }
}