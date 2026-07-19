# ADR 0005 — Limitar concorrência de providers com SemaphoreSlim

## Status

Aceito em 2026-07-19.

## Contexto

A Sprint 6 passou a executar todos os providers concorrentemente com `Task.WhenAll`, reduzindo a duração total de aproximadamente 9 para 5 segundos. Entretanto, `Task.WhenAll` não limita quantas operações podem acessar fontes externas ao mesmo tempo. Com o crescimento da quantidade de providers, iniciar todas as coletas simultaneamente pode pressionar conexões, memória, sockets e serviços de terceiros.

## Decisão

Limitar localmente a execução de providers com uma única instância de `SemaphoreSlim` por execução de `CollectEventsCommandHandler`.

- O limite inicial é `MaximumConcurrency = 5`.
- Todas as tasks compartilham a mesma instância.
- Cada provider aguarda uma permissão com `WaitAsync(cancellationToken)`.
- A permissão é devolvida com `Release()` em `finally`.
- O semáforo é descartado por escopo com `using`.
- `Task.WhenAll`, a agregação posterior e a persistência única são preservados.
- O cronômetro individual começa depois da aquisição, separando tempo de espera de tempo de execução do provider.

## Evidência

O limite foi comprovado temporariamente com valor 2: dois providers iniciaram, o terceiro aguardou uma vaga e a duração total ficou em aproximadamente 6 segundos. Após a comprovação, o limite definitivo voltou para 5; com três providers, a duração total ficou em aproximadamente 5 segundos e 6 eventos foram persistidos.

Commit: `6fb7c67` — `refactor(fight-events): limit concurrent provider execution`.

## Alternativas consideradas

- Executar todos os providers sem limite: menor complexidade, mas risco de crescimento descontrolado.
- Voltar à execução sequencial: limita naturalmente, mas perde o ganho de desempenho validado na Sprint 6.
- Usar uma fila ou scheduler distribuído: pode ser necessário no futuro, mas não resolve com simplicidade o limite local atual.

## Consequências

A coleta mantém concorrência com capacidade máxima local e cancelamento cooperativo. O valor fixo 5 é uma decisão inicial e poderá ser externalizado para configuração quando houver necessidade operacional.

Esta decisão não impede duas coletas completas simultâneas nem coordena múltiplas instâncias do Worker. Sobreposição de execuções e distributed lock permanecem decisões futuras. Falhas parciais, timeout e retry também permanecem fora deste ADR.