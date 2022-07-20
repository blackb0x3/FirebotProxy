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
        using (_logger.BeginScope(nameof(HealthCheck)))
        {
            _logger.LogInformation("health check called");
        }

        return Results.Ok("FirebotProxy is OK!");
    }
}