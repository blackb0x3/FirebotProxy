using System.ComponentModel;

namespace FirebotProxy.Domain.InternalModels.GetViewerChatPlot;

public enum ChartType
{
    [Description("bar")]
    VerticalBar,
    [Description("horizontalBar")]
    HorizontalBar,
    [Description("line")]
    Line,
    [Description("radar")]
    Radar,
    [Description("pie")]
    Pie,
    [Description("doughnut")]
    Doughnut,
    [Description("polarArea")]
    Polar,
    [Description("scatter")]
    Scatter,
}