# Roadmap inicial

## Curto prazo

- Criar testes para o handler e adaptadores fake existentes.
- Definir uma primeira fonte autorizada de eventos e seu contrato de dados.
- Definir modelo de persistência, chave de identidade/deduplicação e comportamento de upsert.
- Implementar um provider real e persistência real antes de expandir fontes.

## Médio prazo

- Introduzir normalização, validação e deduplicação entre fontes.
- Executar coleta periodicamente com configuração de cadência e cancelamento correto.
- Implementar isolamento de falhas, timeout, retry e resultado por provider.
- Criar logs estruturados, métricas e health checks.

## Longo prazo

- Integrar eventos tratados ao produto PowerGrind por contrato explícito.
- Adicionar fontes conforme qualidade, termos de uso e custo operacional.
- Avaliar mensageria ou paralelismo limitado somente quando volume/cadência justificarem.
## Atualização — Sprint 4 concluída

- Múltiplos providers fake foram validados em execução sequencial, com total de 6 eventos persistidos pelo repositório fake.
- O próximo passo é a Sprint 5, de observabilidade básica. A execução continuará sequencial até uma decisão explícita na Sprint 6 para introduzir `Task.WhenAll`.

## Atualização — Sprint 5 concluída

- A execução sequencial agora possui métricas individuais por provider e uma visão total da coleta.
- A linha de base validada foi de aproximadamente 9 segundos para os três providers fake.
- A Sprint 6 usará essa linha de base para comparar a execução concorrente com `Task.WhenAll`.

## Atualização — Sprint 6 concluída

- Providers agora executam concorrentemente com `Task.WhenAll`.
- A linha de base caiu de aproximadamente 9 segundos sequenciais para 5 segundos concorrentes.
- A Sprint 7 adicionará limite de concorrência sem remover `Task.WhenAll`.

## Revisão de cadência — 2026-07-19

As sprints deixam de ser separadas por mecanismo técnico e passam a entregar capacidades completas. Os passos menores continuam existindo como checkpoints internos de aprendizado e revisão.

### Sprint 8 — Motor de coleta resiliente

Falhas parciais, resultado por provider, timeout, configurações e testes automatizados essenciais.

### Sprint 9 — Primeiro provider real

Seleção da fonte, `HttpClientFactory`, modelo externo, `CollectedEvent`, parser, normalização mínima e testes HTTP controlados. Retry será introduzido com base nos erros transitórios dessa integração.

### Sprint 10 — Persistência idempotente

PostgreSQL, decisão EF Core/Dapper, migration, identidade, deduplicação inicial, upsert e testes de integração.

### Sprint 11 — Execução operacional

Agendamento configurável, proteção contra sobreposição, Docker, configuração por ambiente, logs e health check.

### Sprint 12 — MVP integrado

Consulta dos eventos pelo produto, filtros básicos, revisão de observabilidade, documentação operacional e validação ponta a ponta.

Esta revisão substitui o sequenciamento antigo das Sprints 8 a 22 como plano vigente, preservando-o apenas como histórico da evolução do planejamento.