using FirebotProxy.Domain.InternalModels.GetViewerChatPlot;
using FirebotProxy.Domain.PrimaryPorts.GetViewerChatPlot;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Extensions;
using FirebotProxy.SecondaryPorts.GetChatMessages;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OneOf;
using QuickChart;

namespace FirebotProxy.Domain.Adapters;

internal class GetViewerChatPlotRequestHandler : IRequestHandler<GetViewerChatPlotRequest,
    OneOf<GetViewerChatPlotResponse, ValidationRepresentation, ErrorRepresentation>>
{
    private const string Iso8601DateFormat = "yyyy-MM-dd";

    private readonly ILogger<GetViewerChatPlotRequestHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IValidator<GetViewerChatPlotRequest> _validator;

    public GetViewerChatPlotRequestHandler(ILogger<GetViewerChatPlotRequestHandler> logger, IMediator mediator, IValidator<GetViewerChatPlotRequest> validator)
    {
        _logger = logger;
        _mediator = mediator;
        _validator = validator;
    }

    public async Task<OneOf<GetViewerChatPlotResponse, ValidationRepresentation, ErrorRepresentation>> Handle(
        GetViewerChatPlotRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInfo(new { msg = "Handler called", request, handler = nameof(LogChatMessageCommandHandler) });

        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new ValidationRepresentation(validationResult);
            }

            var result = await HandleInternal(request, cancellationToken);

            return result.Match<OneOf<GetViewerChatPlotResponse, ValidationRepresentation, ErrorRepresentation>>(
                success => success,
                validationResult => validationResult
            );
        }
        catch (Exception e)
        {
            const string msg = "Failed to get viewer's chat plot";

            _logger.LogError(new
            {
                msg,
                request,
                handler = nameof(GetViewerChatPlotRequestHandler),
                exception = e.Message,
                e.StackTrace
            });

            return new ErrorRepresentation { Message = msg };
        }
    }

    private async Task<OneOf<GetViewerChatPlotResponse, ValidationRepresentation>> HandleInternal(GetViewerChatPlotRequest request,
        CancellationToken cancellationToken)
    {
        var getChatMessagesBySenderQuery = new GetChatMessagesBySenderQuery { SenderUsername = request.ViewerUsername };

        var chatMessages = await _mediator.Send(getChatMessagesBySenderQuery, cancellationToken);

        // no point continuing if the viewer has no messages
        if (!chatMessages.Any())
        {
            return new ValidationRepresentation($"Viewer {request.ViewerUsername} has not posted to chat.");
        }

        var dateGroupedMessages = chatMessages.GroupBy(cm => cm.Timestamp.Date.ToString(Iso8601DateFormat))
            .OrderBy(grp => grp.Key)
            .ToDictionary(grp => grp.Key, grp => grp.Count());

        // no point continuing if the viewer doesn't have enough days of posts to make a meaningful chart
        if (!ViewerHasAtLeastTwoDaysOfPosts(dateGroupedMessages))
        {
            return new ValidationRepresentation($"Viewer {request.ViewerUsername} does not have at least 2 days of posts.");
        }

        var chart = CreateQuickChartPayload(dateGroupedMessages, request.ViewerUsername, request.ChartType);

        var url = chart.GetShortUrl();

        return new GetViewerChatPlotResponse
        {
            ChartUrl = url
        };
    }

    private static bool ViewerHasAtLeastTwoDaysOfPosts(IDictionary<string, int> dateGroupedMessages)
    {
        return dateGroupedMessages.Keys.Count >= 2;
    }

    private static Chart CreateQuickChartPayload(IDictionary<string, int> dateGroupedMessages, string viewerUsername, string chartType)
    {
        return new Chart
        {
            Width = 1366,
            Height = 768,
            Config = GenerateQuickChartLineChartPayload(dateGroupedMessages, viewerUsername, chartType)
        };
    }

    private static string GenerateQuickChartLineChartPayload(IDictionary<string, int> dateGroupedMessages, string viewerUsername, string chartType)
    {
        Enum.TryParse<ChartType>(chartType, true, out var parsedChartType);
        var dateLabels = GetDateLabels(DateTime.Parse(dateGroupedMessages.Keys.First()), DateTime.Parse(dateGroupedMessages.Keys.Last()));

        var chartData = new ChartPayload<int>
        {
            Type = parsedChartType.GetDescription(),
            Data = new Data<int>
            {
                Labels = dateLabels,
                Datasets = GetDataset(dateGroupedMessages, viewerUsername, dateLabels)
            },
            Options = GetOptions()
        };

        return JsonConvert.SerializeObject(chartData, Formatting.None);
    }

    private static List<string> GetDateLabels(DateTime minDate, DateTime maxDate)
    {
        var dates = new List<string>();

        for (var d = minDate; d <= maxDate; d = d.AddDays(1))
        {
            dates.Add(d.ToString(Iso8601DateFormat));
        }

        return dates;
    }

    private static List<Dataset<int>> GetDataset(IDictionary<string, int> dateGroupedMessages, string viewerUsername, List<string> dateLabels)
    {
        var datasets = new List<Dataset<int>>();

        var dataset = new Dataset<int>
        {
            Label = viewerUsername,
            BackgroundColor = "rgb(50, 134, 230)",
            BorderColor = "rgb(50, 134, 230)",
            Data = new List<int>(),
            Fill = true,
            LineTension = 0.4
        };

        foreach (var dateLabel in dateLabels)
        {
            if (!dateGroupedMessages.TryGetValue(dateLabel, out var msgCount))
            {
                // no date key? then the user didn't post anything on that day
                msgCount = 0;
            }

            dataset.Data.Add(msgCount);
        }

        datasets.Add(dataset);

        return datasets;
    }

    private static Options GetOptions()
    {
        return new Options
        {
            
            Title = new Title
            {
                Display = true,
                Text = "Chat Plot"
            }
        };
    }
}