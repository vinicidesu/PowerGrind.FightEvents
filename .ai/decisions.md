# Registro de decisões

| Situação | Decisão vigente | Evidência |
|---|---|---|
| Estrutura | Clean Architecture em quatro projetos | referências dos `.csproj` |
| Coleta | Worker dispara handler diretamente | `Worker.cs` |
| Fontes | providers são resolvidos por DI via `IEnumerable<IEventProvider>` | `Program.cs`, handler |
| Persistência | fake/log somente | `FakeFightEventRepository.cs` |
| MediatR | pacote e `IRequest` presentes; dispatch completo não vigente | `.csproj`, command/handler/worker |
| Paralelismo/scheduler | planejados, não implementados | código atual || Múltiplos providers fake | Três implementações de `IEventProvider` são resolvidas pelo DI e executadas sequencialmente | Sprint 4 concluída: `Program.cs`, `FakeEventProviderA/B/C` e execução validada com 6 eventos |
| Observabilidade da coleta | Resultados individuais usam `ProviderExecutionResult`; o total é derivado desses resultados | Sprint 5 concluída: handler, response e execução validada |
| Estratégia de execução | Providers permanecem sequenciais na Sprint 5; concorrência será introduzida isoladamente na Sprint 6 | Linha de base validada em aproximadamente 9 segundos |
| Concorrência de providers | Criar uma task por provider e aguardar todas com `Task.WhenAll`; agregar resultados somente após a conclusão | Sprint 6 concluída e validada com 6 eventos em aproximadamente 5 segundos |
| Segurança da agregação | Tasks não escrevem em listas compartilhadas; cada task retorna seus eventos e métricas | `CollectEventsFromProviderAsync` e agregação posterior no handler |
