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

        private const int MaximumConcurrency = 5;

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

            using var semaphore = new SemaphoreSlim(MaximumConcurrency);

            var providerTasks = _providers.Select(provider => CollectEventsFromProviderAsync(provider, semaphore, cancellationToken));            
            var results = await Task.WhenAll(providerTasks);

            foreach (var (events, executionResult) in results)
            {
                providerExecutionResults.Add(executionResult);

                if(executionResult.IsSuccess)
                {
                    eventsCollected.AddRange(events);
                }
            }

            if (eventsCollected.Any())
            {
                await _repository.AddRangeAsync(eventsCollected, cancellationToken);
            }

            totalStopWatch.Stop();

            _logger.LogInformation("Total providers executed: {TotalProvidersExecuted}, Total events collected: {TotalEventsCollected}, Total Duration: {TotalDuration}",
                providerExecutionResults.Count, eventsCollected.Count, totalStopWatch.Elapsed);

            return new CollectEventsResponse(providerExecutionResults);
        }

        private async Task<(IReadOnlyCollection<FightEvent> Events, ProviderExecutionResult ExecutionResult)> CollectEventsFromProviderAsync(IEventProvider provider,
            SemaphoreSlim semaphore, CancellationToken cancellationToken)
        {
            await semaphore.WaitAsync(cancellationToken);

            var stopWatchExecution = new System.Diagnostics.Stopwatch();
            stopWatchExecution.Start();
            try
            {
                _logger.LogInformation("Collecting events from provider: {ProviderName}", provider.Name);

                var events = await provider.GetEventsAsync(cancellationToken);
                stopWatchExecution.Stop();

                var executionResult = new ProviderExecutionResult(
                    Provider: provider.Name,
                    EventsCollected: events.Count,
                    Duration: stopWatchExecution.Elapsed,
                    Status: ProviderExecutionStatus.Succeeded,
                    ErrorMessage: null
                );

                _logger.LogInformation("Events collected from provider: {ProviderName}, Events: {EventsCollected}, Duration: {Duration}",
                    provider.Name, events.Count, stopWatchExecution.Elapsed);

                return (events, executionResult);
            }
            catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
            {
                stopWatchExecution.Stop();

                throw;
            }
            catch(Exception ex)
            {
                stopWatchExecution.Stop();
                _logger.LogError(ex, "Error collecting events from provider: {ProviderName}, duration: {Duration}", provider.Name, stopWatchExecution.Elapsed);

                var executionResult = new ProviderExecutionResult(
                    Provider: provider.Name,
                    EventsCollected: 0,
                    Duration: stopWatchExecution.Elapsed,
                    Status: ProviderExecutionStatus.Failed,
                    ErrorMessage: ex.Message
                );

                return (Array.Empty<FightEvent>(), executionResult);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
