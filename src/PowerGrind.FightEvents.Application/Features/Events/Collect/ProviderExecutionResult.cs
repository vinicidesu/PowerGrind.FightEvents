namespace PowerGrind.FightEvents.Application.Features.Events.Collect
{
    public sealed record ProviderExecutionResult(
        string Provider,
        int EventsCollected,
        TimeSpan Duration);
}
