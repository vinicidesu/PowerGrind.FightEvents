using PowerGrind.FightEvents.Application.Abstractions.Providers;
using PowerGrind.FightEvents.Domain.Entities;

public class FakeEventProvider : IEventProvider
{
    public string Name => "Fake Provider";

    public async Task<IReadOnlyCollection<FightEvent>> GetEventsAsync(
        CancellationToken cancellationToken)
    {
        await Task.Delay(2000, cancellationToken);

        return
        [
            new FightEvent
            {
                Id = Guid.NewGuid(),
                Name = "PowerGrind Open",
                OrganizationName = "PowerGrind",
                Date = new DateTime(2027, 5, 18),
                Location = "Brasil",

                SourceUrl = new Uri("https://powergrind.com")
            },
            new FightEvent
            {
                Id = Guid.NewGuid(),
                Name = "PowerGrind Cup",
                OrganizationName = "PowerGrind",
                Date = new DateTime(2027, 6, 22),
                Location = "Brasil",
                SourceUrl = new Uri("https://powergrind.com")
            }
        ];
    }
}