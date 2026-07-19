# Concluído

- [x] Estrutura .NET 10 em Domain, Application, Infrastructure e Worker.
- [x] Entidade `FightEvent`.
- [x] Contratos de provider e repositório na Application.
- [x] Feature `CollectEvents` que agrega resultados dos providers e chama `AddRangeAsync`.
- [x] Worker que compõe dependências e dispara uma coleta na inicialização.
- [x] Provider e repositório fake para demonstrar o fluxo ponta a ponta.
- [x] Sprint 4: três providers fake resolvidos por DI, executados sequencialmente e validados com 6 eventos persistidos.
- [x] Sprint 5: observabilidade individual e total da coleta, validada com três providers e 6 eventos.
- [x] Sprint 6: providers executados concorrentemente com `Task.WhenAll`, 6 eventos e duração total aproximada de 5 segundos.

- [x] Sprint 7: concorrência limitada com `SemaphoreSlim`, limite definitivo 5 e validação controlada com limite 2. Commit `6fb7c67`.