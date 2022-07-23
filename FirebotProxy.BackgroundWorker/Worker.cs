using Quartz;

namespace FirebotProxy.BackgroundWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ISchedulerFactory _schedulerFactory;

    public Worker(ILogger<Worker> logger, ISchedulerFactory schedulerFactory)
    {
        _logger = logger;
        _schedulerFactory = schedulerFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scheduler = await _schedulerFactory.GetScheduler(stoppingToken);

        await scheduler.Start(stoppingToken);
    }
}