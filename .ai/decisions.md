# Registro de decisões

| Situação | Decisão vigente | Evidência |
|---|---|---|
| Estrutura | Clean Architecture em quatro projetos | referências dos `.csproj` |
| Coleta | Worker dispara handler diretamente | `Worker.cs` |
| Fontes | providers são resolvidos por DI via `IEnumerable<IEventProvider>` | `Program.cs`, handler |
| Persistência | fake/log somente | `FakeFightEventRepository.cs` |
| MediatR | pacote e `IRequest` presentes; dispatch completo não vigente | `.csproj`, command/handler/worker |
| Paralelismo/scheduler | planejados, não implementados | código atual |