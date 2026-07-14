using PowerGrind.FightEvents.Application.Abstractions.Providers;
using PowerGrind.FightEvents.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PowerGrind.FightEvents.Infrastructure.Providers
{
    public class FakeEventProviderC : IEventProvider
    {
        public string Name => "Fake Provider C";

        public async Task<IReadOnlyCollection<FightEvent>> GetEventsAsync(
            CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);

            return
            [
                new FightEvent
            {
                Id = Guid.NewGuid(),
                Name = "PowerGrind C Open",
                OrganizationName = "PowerGrind",
                Date = new DateTime(2027, 5, 18),
                Location = "Brasil",

                SourceUrl = new Uri("https://powergrind.com")
            },
            ];
        }
    }
}
