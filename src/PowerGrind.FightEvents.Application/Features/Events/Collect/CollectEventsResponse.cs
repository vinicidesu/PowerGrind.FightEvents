namespace PowerGrind.FightEvents.Application.Features.Events.Collect;

public sealed record CollectEventsResponse(
    IReadOnlyCollection<ProviderExecutionResult> ProviderExecutions)
{
    public int TotalCollectedEvents => ProviderExecutions.Sum(p => p.EventsCollected);
}