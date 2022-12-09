using System.Text;
using FirebotProxy.Domain.PrimaryPorts.GetChatWordCloud;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Extensions;
using FirebotProxy.SecondaryPorts.GenerateWordCloud;
using FirebotProxy.SecondaryPorts.GetChatMessageText;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace FirebotProxy.Domain.Adapters;

public class GetChatWordCloudRequestHandler : IRequestHandler<GetChatWordCloudRequest, OneOf<GetChatWordCloudResponse, ValidationRepresentation, ErrorRepresentation>>
{
    private readonly ILogger<GetChatWordCloudRequestHandler> _logger;
    private readonly IValidator<GetChatWordCloudRequest> _validator;
    private readonly IMediator _mediator;

    public GetChatWordCloudRequestHandler(ILogger<GetChatWordCloudRequestHandler> logger, IValidator<GetChatWordCloudRequest> validator, IMediator mediator)
    {
        _logger = logger;
        _validator = validator;
        _mediator = mediator;
    }

    public async Task<OneOf<GetChatWordCloudResponse, ValidationRepresentation, ErrorRepresentation>> Handle(GetChatWordCloudRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await HandleInternal(request, cancellationToken);

            return result.Match<OneOf<GetChatWordCloudResponse, ValidationRepresentation, ErrorRepresentation>>(
                successfulResponse => successfulResponse,
                invalidRequest => invalidRequest
            );
        }
        catch (Exception e)
        {
            const string msg = "Failed to get chat word cloud";

            _logger.LogError(new
            {
                msg,
                request,
                handler = nameof(GetChatWordCloudRequestHandler),
                exception = e.Message,
                e.StackTrace
            });

            return new ErrorRepresentation { Message = msg };
        }
    }

    private async Task<OneOf<GetChatWordCloudResponse, ValidationRepresentation>> HandleInternal(GetChatWordCloudRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ValidationRepresentation(validationResult);
        }

        var chatText = await GetChatText(request.ViewerUsername, request.StreamDate, cancellationToken);

        // TODO inject mapper and implement converter
        WordCloudOptions wordCloudOptions = _mapper.Map<WordCloudOptions>(request.WordCloudSettings);

        var generateWordCloudResponse = await _mediator.Send(new GenerateWordCloudCommand { WordCloudOptions = wordCloudOptions }, cancellationToken);

        if (generateWordCloudResponse.IsT1)
        {
            throw new Exception("QuickChart could not render a word cloud");
        }

        var base64WordCloud = Convert.ToBase64String(Encoding.UTF8.GetBytes(generateWordCloudResponse.AsT0.WordCloudContent));

        // TODO append base64 string to word cloud renderer on blackb0x3 github IO page

        return new GetChatWordCloudResponse
        {
            WordCloudUrl = string.Empty
        };
    }

    private async Task<string> GetChatText(string? viewerUsername, string? streamDate, CancellationToken cancellationToken)
    {
        var chatTextQuery = new GetChatMessageTextQuery
        {
            ViewerUsername = viewerUsername,
        };

        if (streamDate != null)
        {
            chatTextQuery.StreamDate = DateTime.Parse(streamDate);
        }

        return await _mediator.Send(chatTextQuery, cancellationToken);
    }
}