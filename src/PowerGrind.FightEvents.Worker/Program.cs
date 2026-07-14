using PowerGrind.FightEvents.Application.Abstractions.Persistence;
using PowerGrind.FightEvents.Application.Abstractions.Providers;
using PowerGrind.FightEvents.Application.Features.Events.Collect;
using PowerGrind.FightEvents.Infrastructure.Persistence;
using PowerGrind.FightEvents.Infrastructure.Providers;
using PowerGrind.FightEvents.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddScoped<IEventProvider, FakeEventProviderA>();
builder.Services.AddScoped<IEventProvider, FakeEventProviderB>();
builder.Services.AddScoped<IEventProvider, FakeEventProviderC>();
builder.Services.AddScoped<IFightEventRepository, FakeFightEventRepository>();
builder.Services.AddScoped<CollectEventsCommandHandler>();

var host = builder.Build();
host.Run();
