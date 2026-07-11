using MediatR;

namespace PowerGrind.FightEvents.Application.Features.Events.Collect;

public sealed record CollectEventsCommand
    : IRequest<CollectEventsResponse>;