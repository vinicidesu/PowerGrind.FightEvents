using Microsoft.Extensions.Logging;
using PowerGrind.FightEvents.Application.Abstractions.Persistence;
using PowerGrind.FightEvents.Application.Abstractions.Providers;
using PowerGrind.FightEvents.Domain.Entities;

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
            var providerExecutionResults = new List<ProviderExecutionResult>();
            var totalStopWatch = new System.Diagnostics.Stopwatch();
            totalStopWatch.Start();

            foreach (var provider in _providers)
            {
                _logger.LogInformation("Collecting events from provider: {ProviderName}", provider.Name);
                var stopWatchExecution = new System.Diagnostics.Stopwatch();
                stopWatchExecution.Start();

                var events = await provider.GetEventsAsync(cancellationToken);

                eventsCollected.AddRange(events);

                stopWatchExecution.Stop();

                providerExecutionResults.Add(new ProviderExecutionResult(
                    Provider: provider.Name,
                    EventsCollected: events.Count,
                    Duration: stopWatchExecution.Elapsed
                ));

                _logger.LogInformation("Events collected from provider: {ProviderName}, Events: {EventsCollected}, Duration: {Duration}",
                    provider.Name, events.Count, stopWatchExecution.Elapsed);
            }

            await _repository.AddRangeAsync(eventsCollected, cancellationToken);
            totalStopWatch.Stop();

            _logger.LogInformation("Total providers executed: {TotalProvidersExecuted}, Total events collected: {TotalEventsCollected}, Total Duration: {TotalDuration}",
                _providers.Count(), eventsCollected.Count, totalStopWatch.Elapsed);

            return new CollectEventsResponse(providerExecutionResults);
        }
    }
}
