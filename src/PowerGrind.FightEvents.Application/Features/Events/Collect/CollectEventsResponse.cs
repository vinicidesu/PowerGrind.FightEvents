namespace PowerGrind.FightEvents.Application.Features.Events.Collect;

public sealed record CollectEventsResponse(
    IReadOnlyCollection<ProviderExecutionResult> ProviderExecutions)
{
    public int TotalCollectedEvents => ProviderExecutions.Sum(p => p.EventsCollected);
    public int TotalSuccessfulProviders => ProviderExecutions.Count(p => p.Status == ProviderExecutionStatus.Succeeded);
    public int TotalFailedProviders => ProviderExecutions.Count(p => p.Status == ProviderExecutionStatus.Failed);
    public int TotalTimedOutProviders => ProviderExecutions.Count(p => p.Status == ProviderExecutionStatus.TimedOut);
    public int TotalProviders => ProviderExecutions.Count;
    public bool HasFailures => ProviderExecutions.Any(p => p.Status != ProviderExecutionStatus.Succeeded);
}