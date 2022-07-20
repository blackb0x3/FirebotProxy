namespace FirebotProxy.Api.Controllers;

public class HealthCheckController : ProxyControllerBase
{
    private readonly ILogger<HealthCheckController> _logger;

    public HealthCheckController(ILogger<HealthCheckController> logger)
    {
        _logger = logger;
    }

    public IResult HealthCheck()
    {
        return Results.Ok("OK");
    }
}