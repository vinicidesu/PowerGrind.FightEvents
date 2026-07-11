using PowerGrind.FightEvents.Domain.Entities;

namespace PowerGrind.FightEvents.Application.Abstractions.Persistence
{
    public interface IFightEventRepository
    {
        Task<IReadOnlyCollection<FightEvent>> SaveAsync(IReadOnlyCollection<FightEvent> events, CancellationToken cancellationToken);
    }
}
