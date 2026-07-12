using Microsoft.Extensions.Logging;
using PowerGrind.FightEvents.Application.Abstractions.Persistence;
using PowerGrind.FightEvents.Domain.Entities;

namespace PowerGrind.FightEvents.Infrastructure.Persistence
{
    public class FakeFightEventRepository : IFightEventRepository
    {
        private readonly ILogger<FakeFightEventRepository> _logger;

        public FakeFightEventRepository(ILogger<FakeFightEventRepository> logger)
        {
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<FightEvent>> AddRangeAsync(IReadOnlyCollection<FightEvent> events, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Count} events saved.", events.Count);

            return events;
        }
    }
}
