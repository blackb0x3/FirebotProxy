namespace FirebotProxy.Api.Models.Request;

public class RemoveBannedViewerMessagesRequest
{
    public string BannedViewerUsername { get; set; } = null!;
}