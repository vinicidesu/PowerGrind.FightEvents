using PowerGrind.FightEvents.Domain.Entities;

namespace PowerGrind.FightEvents.Application.Abstractions.Providers
{
    public interface IEventProvider
    {
        string Name { get; }

        Task<IReadOnlyCollection<FightEvent>> GetEventsAsync(CancellationToken cancellationToken);
    }
}
