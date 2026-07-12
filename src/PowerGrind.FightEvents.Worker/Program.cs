using PowerGrind.FightEvents.Application.Abstractions.Persistence;
using PowerGrind.FightEvents.Application.Abstractions.Providers;
using PowerGrind.FightEvents.Application.Features.Events.Collect;
using PowerGrind.FightEvents.Infrastructure.Persistence;
using PowerGrind.FightEvents.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddScoped<IEventProvider, FakeEventProvider>();
builder.Services.AddScoped<IFightEventRepository, FakeFightEventRepository>();
builder.Services.AddScoped<CollectEventsCommandHandler>();

var host = builder.Build();
host.Run();
