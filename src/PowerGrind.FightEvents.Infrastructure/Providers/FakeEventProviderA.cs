using PowerGrind.FightEvents.Application.Abstractions.Providers;
using PowerGrind.FightEvents.Domain.Entities;

namespace PowerGrind.FightEvents.Infrastructure.Providers
{
    public class FakeEventProviderA : IEventProvider
    {
        public string Name => "Fake Provider A";

        public async Task<IReadOnlyCollection<FightEvent>> GetEventsAsync(
            CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);

            return
            [
                new FightEvent
            {
                Id = Guid.NewGuid(),
                Name = "PowerGrind A Open",
                OrganizationName = "PowerGrind",
                Date = new DateTime(2027, 5, 18),
                Location = "Brasil",

                SourceUrl = new Uri("https://powergrind.com")
            },
            new FightEvent
            {
                Id = Guid.NewGuid(),
                Name = "PowerGrind A Cup",
                OrganizationName = "PowerGrind",
                Date = new DateTime(2027, 6, 22),
                Location = "Brasil",
                SourceUrl = new Uri("https://powergrind.com")
            }
            ];
        }
    }
}