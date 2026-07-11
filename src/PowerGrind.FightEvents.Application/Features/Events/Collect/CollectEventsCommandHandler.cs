using Microsoft.Extensions.Logging;
using PowerGrind.FightEvents.Application.Abstractions.Persistence;
using PowerGrind.FightEvents.Application.Abstractions.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PowerGrind.FightEvents.Application.Features.Events.Collect
{
    public class CollectEventsCommandHandler
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<IEventProvider> _providers;
        private readonly IFightEventRepository _repository;

        public CollectEventsCommandHandler(IEnumerable<IEventProvider> providers, IFightEventRepository repository, ILogger<CollectEventsCommandHandler> logger)
        {
            _providers = providers;
            _logger = logger;
            _repository = repository;
        }


    }
}
