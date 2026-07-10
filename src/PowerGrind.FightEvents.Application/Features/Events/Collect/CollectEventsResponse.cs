namespace PowerGrind.FightEvents.Application.Features.Events.Collect;

public sealed record CollectEventsResponse(
    int TotalProviders,
    int TotalEvents
);