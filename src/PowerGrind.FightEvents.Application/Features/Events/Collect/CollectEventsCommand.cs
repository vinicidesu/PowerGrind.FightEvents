using PowerGrind.FightEvents.Application.Abstractions.Messaging;

namespace PowerGrind.FightEvents.Application.Features.Events.Collect;

public sealed record CollectEventsCommand
    : ICommand<CollectEventsResponse>;