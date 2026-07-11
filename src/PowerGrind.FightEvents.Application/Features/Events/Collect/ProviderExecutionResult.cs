using System;
using System.Collections.Generic;
using System.Text;

namespace PowerGrind.FightEvents.Application.Features.Events.Collect
{
    public sealed record ProviderExecutionResult(
        string Provider,
        bool Success,
        int EventsCollected,
        TimeSpan Duration,
        Exception? Exception = null);
}
