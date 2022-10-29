using FirebotProxy.Models;
using Quartz;

namespace FirebotProxy.BackgroundWorker.Jobs.RemoveExpiredChatMessages;

public class RemoveExpiredChatMessagesSchedule : ScheduleBase
{
    public override int IntervalValue => 5;

    public override IntervalUnit IntervalUnit => IntervalUnit.Minute;
}