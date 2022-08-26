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
    [Description("scatter")]
    Scatter,
    /*
     * TODO fix single colour issue on these chart types
    [Description("radar")]
    Radar,
    [Description("pie")]
    Pie,
    [Description("doughnut")]
    Doughnut,
    [Description("polarArea")]
    Polar,*/
}