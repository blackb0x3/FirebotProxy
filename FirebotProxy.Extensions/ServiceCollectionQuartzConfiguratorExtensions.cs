using FirebotProxy.Models;
using Quartz;

namespace FirebotProxy.Extensions;

public static class ServiceCollectionQuartzConfiguratorExtensions
{
    public static IServiceCollectionQuartzConfigurator ScheduleJob<TJob, TSchedule>(
        this IServiceCollectionQuartzConfigurator qc) where TJob : IJob where TSchedule : ISchedule, new()
    {
        return qc.ScheduleJob<TJob>(new TSchedule().Trigger);
    }
}