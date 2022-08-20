using Quartz;

namespace FirebotProxy.Models;

public interface ISchedule
{
    void Trigger(ITriggerConfigurator trigger);

    int IntervalValue { get; }

    IntervalUnit IntervalUnit { get; }
}