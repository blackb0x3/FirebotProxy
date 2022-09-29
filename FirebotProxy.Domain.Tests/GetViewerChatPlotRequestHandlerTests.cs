using FirebotProxy.Domain.Adapters;
using FirebotProxy.Domain.InternalModels.GetViewerChatPlot;
using FirebotProxy.Domain.PrimaryPorts.GetViewerChatPlot;
using FirebotProxy.Domain.Representations;
using FirebotProxy.Domain.Tests.FakeHandlers;
using FirebotProxy.Domain.Validators;
using FirebotProxy.TestBase;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;

namespace FirebotProxy.Domain.Tests;

[TestFixture]
public class A_GetViewerChatPlot_Request_Handler_Returns_A_Chat_Plot_For_A_Viewer
{
    [Test]
    [Description("For when the viewer doesn't supply a chart type")]
    public async Task When_The_Default_Chart_Type_Is_Supplied()
    {
        var mediator = new MediatRFactory(typeof(A_GetViewerChatPlot_Request_Handler_Returns_A_Chat_Plot_For_A_Viewer).Assembly)
            .AddSingletonHandler(new FakeGetChatMessagesBySenderQueryHandler("test_user", DateTime.UtcNow, DateTime.UtcNow.AddDays(10)))
            .Build();

        var handler = new GetViewerChatPlotRequestHandler(new NullLogger<GetViewerChatPlotRequestHandler>(), mediator, new GetViewerChatPlotRequestValidator());

        var response = await handler.Handle(new GetViewerChatPlotRequest { ViewerUsername = "test_viewer", ChartType = "Line" }, CancellationToken.None);

        response.Value.Should().BeOfType<GetViewerChatPlotResponse>();

        response.AsT0.ChartUrl.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    [TestCase("VerticalBar")]
    [TestCase("HorizontalBar")]
    [TestCase("Scatter")]
    public async Task When_A_Valid_Chart_Type_That_Is_Not_The_Default_Is_Supplied(string validChartType)
    {
        var mediator = new MediatRFactory(typeof(A_GetViewerChatPlot_Request_Handler_Returns_A_Chat_Plot_For_A_Viewer).Assembly)
            .AddSingletonHandler(new FakeGetChatMessagesBySenderQueryHandler("test_user", DateTime.UtcNow, DateTime.UtcNow.AddDays(10)))
            .Build();

        var handler = new GetViewerChatPlotRequestHandler(new NullLogger<GetViewerChatPlotRequestHandler>(), mediator, new GetViewerChatPlotRequestValidator());

        var response = await handler.Handle(new GetViewerChatPlotRequest { ViewerUsername = "test_viewer", ChartType = validChartType }, CancellationToken.None);

        response.Value.Should().BeOfType<GetViewerChatPlotResponse>();

        response.AsT0.ChartUrl.Should().NotBeNullOrWhiteSpace();
    }
}

[TestFixture]
public class A_GetViewerChatPlot_Request_Handler_Does_Not_Return_A_Chat_Plot_For_A_Viewer
{
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("         ")]
    public async Task When_The_Viewer_Username_Is_Not_Supplied(string viewerUsername)
    {
        var mediator = new MediatRFactory(typeof(A_GetViewerChatPlot_Request_Handler_Returns_A_Chat_Plot_For_A_Viewer).Assembly)
            .AddSingletonHandler(new FakeGetChatMessagesBySenderQueryHandler("test_user", DateTime.UtcNow, DateTime.UtcNow.AddDays(10)))
            .Build();

        var handler = new GetViewerChatPlotRequestHandler(new NullLogger<GetViewerChatPlotRequestHandler>(), mediator, new GetViewerChatPlotRequestValidator());

        var response = await handler.Handle(new GetViewerChatPlotRequest { ViewerUsername = viewerUsername, ChartType = "Scatter" }, CancellationToken.None);

        response.Value.Should().BeOfType<ValidationRepresentation>();

        response.AsT1.Errors.Should().HaveCount(1);
        response.AsT1.Errors.First().Should().Be("ViewerUsername : Viewer username must not be null or empty.");
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("         ")]
    public async Task When_The_Chart_Type_Is_Not_Provided(string chartType)
    {
        var mediator = new MediatRFactory(typeof(A_GetViewerChatPlot_Request_Handler_Returns_A_Chat_Plot_For_A_Viewer).Assembly)
            .AddSingletonHandler(new FakeGetChatMessagesBySenderQueryHandler("test_user", DateTime.UtcNow, DateTime.UtcNow.AddDays(10)))
            .Build();

        var handler = new GetViewerChatPlotRequestHandler(new NullLogger<GetViewerChatPlotRequestHandler>(), mediator, new GetViewerChatPlotRequestValidator());

        var response = await handler.Handle(new GetViewerChatPlotRequest { ViewerUsername = "test_viewer", ChartType = chartType }, CancellationToken.None);

        response.Value.Should().BeOfType<ValidationRepresentation>();

        response.AsT1.Errors.Should().HaveCount(1);
        response.AsT1.Errors.First().Should().Be($"ChartType : Unsupported chart type. Valid chart types are: {string.Join(", ", Enum.GetValues<ChartType>()).ToLower()}");
    }

    [Test]
    [TestCase("rAnD0mN0n53n53")]
    [TestCase("Radar")]
    [TestCase("Pie")]
    [TestCase("Doughnut")]
    [TestCase("Polar")]
    [TestCase("Scatter Graph")] // should be Scatter
    [TestCase("Line Chart")] // should be Line
    public async Task When_The_Provided_Chart_Type_Is_Invalid(string invalidChartType)
    {
        var mediator = new MediatRFactory(typeof(A_GetViewerChatPlot_Request_Handler_Returns_A_Chat_Plot_For_A_Viewer).Assembly)
            .AddSingletonHandler(new FakeGetChatMessagesBySenderQueryHandler("test_user", DateTime.UtcNow, DateTime.UtcNow.AddDays(10)))
            .Build();

        var handler = new GetViewerChatPlotRequestHandler(new NullLogger<GetViewerChatPlotRequestHandler>(), mediator, new GetViewerChatPlotRequestValidator());

        var response = await handler.Handle(new GetViewerChatPlotRequest { ViewerUsername = "test_viewer", ChartType = invalidChartType }, CancellationToken.None);

        response.Value.Should().BeOfType<ValidationRepresentation>();

        response.AsT1.Errors.Should().HaveCount(1);
        response.AsT1.Errors.First().Should().Be($"ChartType : Unsupported chart type. Valid chart types are: {string.Join(", ", Enum.GetValues<ChartType>()).ToLower()}");
    }
}