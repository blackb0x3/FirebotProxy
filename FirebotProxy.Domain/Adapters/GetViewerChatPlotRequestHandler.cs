using System.Text;
using FirebotProxy.Data.Entities;
using FirebotProxy.Domain.InternalModels.GetViewerChatPlot;
using FirebotProxy.Domain.PrimaryPorts.GetViewerChatPlot;
using FirebotProxy.Domain.Representations;
using FirebotProxy.SecondaryPorts.GetChatMessages;
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

    public GetViewerChatPlotRequestHandler(ILogger<GetViewerChatPlotRequestHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<OneOf<GetViewerChatPlotResponse, ValidationRepresentation, ErrorRepresentation>> Handle(
        GetViewerChatPlotRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var getChatMessagesBySenderQuery = new GetChatMessagesBySenderQuery { SenderUsername = request.ViewerUsername };

            var chatMessages = await _mediator.Send(getChatMessagesBySenderQuery, cancellationToken);

            if (!chatMessages.Any())
            {
                return new ValidationRepresentation($"Viewer {request.ViewerUsername} has not posted to chat.");
            }

            var dateGroupedMessages = chatMessages.GroupBy(cm => cm.Timestamp.Date.ToString(Iso8601DateFormat))
                .OrderBy(grp => grp.Key)
                .ToDictionary(grp => grp.Key, grp => grp.Count());

            if (dateGroupedMessages.Select(x => x.Key).Count() < 2)
            {
                return new ValidationRepresentation($"Viewer {request.ViewerUsername} does not have at least 2 days of posts.");
            }

            var chart = CreateQuickChartPayload(dateGroupedMessages, request.ViewerUsername);

            var url = chart.GetShortUrl();

            return new GetViewerChatPlotResponse
            {
                ChartUrl = url
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogError(e.StackTrace);

            return new ErrorRepresentation { Message = e.Message };
        }
    }

    private static Chart CreateQuickChartPayload(IDictionary<string, int> dateGroupedMessages, string viewerUsername)
    {
        return new Chart
        {
            Width = 1366,
            Height = 768,
            Config = GenerateQuickChartLineChartPayload(dateGroupedMessages, viewerUsername)
        };
    }

    private static string GenerateQuickChartLineChartPayload(IDictionary<string, int> dateGroupedMessages, string viewerUsername)
    {
        var dateLabels = GetDateLabels(DateTime.Parse(dateGroupedMessages.Keys.First()), DateTime.Parse(dateGroupedMessages.Keys.Last()));

        var chartData = new ChartPayload<int>
        {
            Type = "bar",
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
            dates.Add(d.ToString());
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
            Fill = false,
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