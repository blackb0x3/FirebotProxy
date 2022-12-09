using System.Text;
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

        var chatText = await GetChatText(request.ViewerUsername, request.StreamDate, cancellationToken);

        WordCloudOptions wordCloudOptions = _mapper.Map<WordCloudOptions>(request.WordCloudSettings);
        wordCloudOptions.Text = chatText;

        var generateWordCloudResponse = await _mediator.Send(new GenerateWordCloudCommand { WordCloudOptions = wordCloudOptions }, cancellationToken);

        if (generateWordCloudResponse.IsT1)
        {
            throw new Exception("QuickChart could not render a word cloud");
        }

        var base64WordCloud = generateWordCloudResponse.AsT0.WordCloudContent.ToUrlSafeBase64String();
        var base64WordCloudUrl = $"https://blackb0x3.github.io?base64SvgContent={base64WordCloud}";

        var shortenedUrl = await _mediator.Send(new ShortenUrlCommand { UrlToShorten = base64WordCloudUrl }, cancellationToken);

        return new GetChatWordCloudResponse
        {
            WordCloudUrl = shortenedUrl
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