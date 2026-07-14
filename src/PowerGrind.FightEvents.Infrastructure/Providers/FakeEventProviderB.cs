using PowerGrind.FightEvents.Application.Abstractions.Providers;
using PowerGrind.FightEvents.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PowerGrind.FightEvents.Infrastructure.Providers
{
    public class FakeEventProviderB : IEventProvider
    {
        public string Name => "Fake Provider B";

        public async Task<IReadOnlyCollection<FightEvent>> GetEventsAsync(
            CancellationToken cancellationToken)
        {
            await Task.Delay(3000, cancellationToken);

            return
            [
                new FightEvent
            {
                Id = Guid.NewGuid(),
                Name = "PowerGrind B Open",
                OrganizationName = "PowerGrind",
                Date = new DateTime(2027, 5, 18),
                Location = "Brasil",

                SourceUrl = new Uri("https://powergrind.com")
            },
            new FightEvent
            {
                Id = Guid.NewGuid(),
                Name = "PowerGrind B Cup",
                OrganizationName = "PowerGrind",
                Date = new DateTime(2027, 6, 22),
                Location = "Brasil",
                SourceUrl = new Uri("https://powergrind.com")
            },
            new FightEvent
            {
                Id = Guid.NewGuid(),
                Name = "PowerGrind B Champ",
                OrganizationName = "PowerGrind",
                Date = new DateTime(2027, 5, 18),
                Location = "Brasil",

                SourceUrl = new Uri("https://powergrind.com")
            },
            ];
        }
    }
}
