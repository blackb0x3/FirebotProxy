using Quartz;

namespace FirebotProxy.Models;

public abstract class ScheduleBase : ISchedule
{
    public abstract int IntervalValue { get; }

    public abstract IntervalUnit IntervalUnit { get; }

    public virtual void Trigger(ITriggerConfigurator trigger)
    {
        trigger.StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(1)))
            .WithDailyTimeIntervalSchedule(x => x.WithInterval(IntervalValue, IntervalUnit));
    }
}