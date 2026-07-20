namespace PowerGrind.FightEvents.Application.Features.Events.Collect
{
    public sealed record ProviderExecutionResult(
        string Provider,
        int EventsCollected,
        TimeSpan Duration,
        ProviderExecutionStatus Status,
        string? ErrorMessage = null)
    {
        public bool IsSuccess => Status == ProviderExecutionStatus.Succeeded;
        public bool IsFailed => Status == ProviderExecutionStatus.Failed;
        public bool IsTimedOut => Status == ProviderExecutionStatus.TimedOut;
    }
}
