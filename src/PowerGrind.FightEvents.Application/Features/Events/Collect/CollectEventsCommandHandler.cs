using Microsoft.Extensions.Logging;
using PowerGrind.FightEvents.Application.Abstractions.Persistence;
using PowerGrind.FightEvents.Application.Abstractions.Providers;
using PowerGrind.FightEvents.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PowerGrind.FightEvents.Application.Features.Events.Collect
{
    public class CollectEventsCommandHandler
    {
        private readonly ILogger<CollectEventsCommandHandler> _logger;
        private readonly IEnumerable<IEventProvider> _providers;
        private readonly IFightEventRepository _repository;

        public CollectEventsCommandHandler(IEnumerable<IEventProvider> providers, IFightEventRepository repository, ILogger<CollectEventsCommandHandler> logger)
        {
            _providers = providers;
            _logger = logger;
            _repository = repository;
        }

        public async Task<CollectEventsResponse> Handle(CollectEventsCommand command, CancellationToken cancellationToken)
        {
            var eventsCollected = new List<FightEvent>();

            int providersExecuted = 0;

            foreach (var provider in _providers)
            {
                _logger.LogInformation("Collecting events from provider: {ProviderName}", provider.Name);

                var events = await provider.GetEventsAsync(cancellationToken);

                eventsCollected.AddRange(events);

                providersExecuted++;
            }

            await _repository.AddRangeAsync(eventsCollected, cancellationToken);

            return new CollectEventsResponse(eventsCollected.Count);
        }
    }
}
