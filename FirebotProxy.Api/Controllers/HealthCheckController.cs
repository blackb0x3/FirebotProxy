using Microsoft.AspNetCore.Mvc;

namespace FirebotProxy.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthCheckController : ControllerBase
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