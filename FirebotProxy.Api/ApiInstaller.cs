using System.Reflection;
using FirebotProxy.Api.Middleware;
using FirebotProxy.BackgroundWorker;
using FirebotProxy.BackgroundWorker.Jobs.RemoveExpiredChatMessages;
using FirebotProxy.Extensions;
using Quartz;

namespace FirebotProxy.Api;

public class ApiInstaller
{
    public static void Install(IServiceCollection services)
    {
        AddMiddleware(services);
        AddBackgroundWorker(services);
    }

    private static void AddMiddleware(IServiceCollection services)
    {
        services.AddTransient<FirebotRequestMiddleware>();
    }

    private static void AddBackgroundWorker(IServiceCollection services)
    {
        services.AddQuartz(q =>
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
    }

    private static readonly Assembly ApiProjectAssembly = typeof(ApiInstaller).Assembly;
}