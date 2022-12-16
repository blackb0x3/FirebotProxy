using FirebotProxy.Domain.PrimaryPorts.GetChatWordCloud;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Extensions;
using FirebotProxy.SecondaryPorts.GenerateWordCloud;
using FirebotProxy.SecondaryPorts.GetChatMessageText;
using FirebotProxy.SecondaryPorts.ShortenUrl;
using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace FirebotProxy.Domain.Adapters;

public class GetChatWordCloudRequestHandler : IRequestHandler<GetChatWordCloudRequest, OneOf<GetChatWordCloudResponse, ValidationRepresentation, ErrorRepresentation>>
{
    private readonly ILogger<GetChatWordCloudRequestHandler> _logger;
    private readonly IValidator<GetChatWordCloudRequest> _validator;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public GetChatWordCloudRequestHandler(ILogger<GetChatWordCloudRequestHandler> logger, IValidator<GetChatWordCloudRequest> validator, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _validator = validator;
        _mediator = mediator;
        _mapper = mapper;
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

        var chatText = await GetChatText(request, cancellationToken);

        var wordCloudOptions = _mapper.Map<WordCloudOptions>(request.WordCloudSettings);
        wordCloudOptions.Text = chatText;

        if (request.WordCloudSettings.WordHexColours.Length > 0)
        {
            wordCloudOptions.WordHexColours = $"[\"{string.Join("\",\"", request.WordCloudSettings.WordHexColours)}\"]";
        }

        var generateWordCloudResponse = await _mediator.Send(new GenerateWordCloudCommand { WordCloudOptions = wordCloudOptions }, cancellationToken);

        generateWordCloudResponse.Switch(
            success => { }, // carry on as normal
            failure => throw new Exception("QuickChart could not render a word cloud") 
        );

        var base64WordCloud = generateWordCloudResponse.AsT0.WordCloudContent.ToUrlSafeBase64String();
        var base64WordCloudUrl = $"https://blackb0x3.github.io?base64SvgContent={base64WordCloud}";

        var shortenUrlResponse = await _mediator.Send(new ShortenUrlCommand { UrlToShorten = base64WordCloudUrl }, cancellationToken);

        return shortenUrlResponse.Match(
            success => new GetChatWordCloudResponse { WordCloudUrl = success.ShortenedUrl },
            failure => throw new Exception("HideUri could not generate shortened word cloud URL")
        );
    }

    private async Task<string> GetChatText(GetChatWordCloudRequest request, CancellationToken cancellationToken)
    {
        var chatTextQuery = new GetChatMessageTextQuery
        {
            ViewerUsername = request.ViewerUsername,
            NumberOfWordsToTake = 10
        };

        if (request.StreamDate != null)
        {
            chatTextQuery.StreamDate = DateTime.Parse(request.StreamDate);
        }

        return await _mediator.Send(chatTextQuery, cancellationToken);
    }
}