using PowerGrind.FightEvents.Domain.Entities;

namespace PowerGrind.FightEvents.Domain.Interfaces
{
    public interface IEventProvider
    {
        string Name { get; }

        Task<IReadOnlyCollection<FightEvent>> GetEventsAsync(CancellationToken cancellationToken);
    }
}
