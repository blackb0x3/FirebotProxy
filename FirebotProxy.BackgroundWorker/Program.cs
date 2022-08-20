using FirebotProxy.BackgroundWorker;
using FirebotProxy.BackgroundWorker.Jobs.RemoveExpiredChatMessages;
using FirebotProxy.Data.Access;
using FirebotProxy.Domain.IoC;
using FirebotProxy.Extensions;
using FirebotProxy.Helpers;
using FirebotProxy.Infrastructure.IoC;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog((_, lc) => lc
        .WriteTo.Console()
        .WriteTo.Seq("http://localhost:5341")
    )
    .ConfigureServices(services =>
    {
        services.AddDbContext<FirebotProxyContext>(options =>
            options.UseSqlite(DatabasePathHelper.GetSqliteConnectionString()));

        DomainInstaller.Install(services);
        InfrastructureInstaller.Install(services);

        services.AddQuartz((IServiceCollectionQuartzConfigurator q) =>
        {
            q.SchedulerId = "Scheduler-Core";

            q.InterruptJobsOnShutdown = true;

            q.UseMicrosoftDependencyInjectionJobFactory();

            // auto-interrupt long-running jobs
            q.UseJobAutoInterrupt(options =>
            {
                options.DefaultMaxRunTime = TimeSpan.FromMinutes(1);
            });

            q.ScheduleJob<RemoveExpiredChatMessagesJob, RemoveExpiredChatMessagesSchedule>();
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();