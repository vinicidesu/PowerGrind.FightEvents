# Sprint 1 proposta — Da demonstração ao primeiro dado persistido

## Objetivo

Substituir o fluxo demonstrativo por uma coleta de uma fonte definida e persistida de forma idempotente.

## Itens

- Definir primeira fonte autorizada e o contrato mínimo de evento.
- Criar modelo de persistência, migrations e estratégia de identidade/upsert.
- Implementar provider real com cancelamento e logs.
- Criar testes para o handler e para a persistência/provider inicial.

## Riscos e dependências

- Termos de uso, estabilidade e acesso da fonte externa.
- Critério de duplicidade entre fontes e decisão sobre banco compartilhado ou próprio.
- Contrato de integração com o restante do PowerGrind ainda não definido.
---

# Sprint 4 concluída — Múltiplos Providers

## Objetivo

Validar a resolução de múltiplas implementações de `IEventProvider` pelo container de DI e a execução sequencial de todas pelo handler.

## Entregue

- `FakeEventProviderA`: delay de 1 segundo e 2 eventos.
- `FakeEventProviderB`: delay de 3 segundos e 3 eventos.
- `FakeEventProviderC`: delay de 5 segundos e 1 evento.
- Registro das três implementações no DI do Worker.
- Execução validada: os três providers foram chamados uma vez, com nomes distintos nos logs, e o repositório fake recebeu 6 eventos.
- Solution compilada sem avisos ou erros.

## Fora do escopo

`Task.WhenAll`, `SemaphoreSlim`, retry, timeout, provider real, banco de dados e deduplicação.

## Próxima sprint

Sprint 5 — Observabilidade básica: logs de início/fim, duração e quantidade por provider, duração total e evolução da resposta de coleta.

---

# Sprint 5 em andamento — Observabilidade básica

## Objetivo

Tornar a execução sequencial dos providers observável, sem alterar sua estratégia de concorrência nem o comportamento de persistência.

## Itens

- Medir a duração de cada provider com `Stopwatch`.
- Registrar logs de início e fim por provider, incluindo quantidade de eventos e duração.
- Medir e registrar a duração total da coleta.
- Criar `ProviderExecutionResult` na feature `Collect` para representar o resultado de uma execução bem-sucedida.
- Evoluir `CollectEventsResponse` para expor os resultados por provider e o total coletado.
- Manter a execução sequencial e o cancelamento existente.
- Criar ou atualizar testes proporcionais ao comportamento novo, se a estrutura de testes já estiver disponível.
- Atualizar documentação e criar commit ao concluir.

## Fora do escopo

`Task.WhenAll`, `SemaphoreSlim`, falhas parciais, retry, timeout, provider real, normalização, deduplicação e persistência real.

## Critérios de aceite

- Cada provider gera log de início e de conclusão com nome, quantidade e duração.
- Há um log final com a duração total e o total coletado.
- A resposta da coleta contém resultados individuais suficientes para a próxima sprint evoluir falhas parciais.
- Os três providers fake continuam sendo executados sequencialmente e persistem 6 eventos.
- A solution compila sem erros ou avisos.

## Commit esperado

`feat(fight-events): add provider execution observability`

---

# Sprint 5 concluída — Observabilidade básica

## Entregue

- Medição individual com `Stopwatch`.
- Logs de início e conclusão por provider.
- Quantidade e duração por provider.
- `ProviderExecutionResult` na feature `Collect`.
- `CollectEventsResponse` enriquecido com resultados individuais e total calculado.
- Medição e log da execução total.
- Fluxo sequencial validado com três providers e 6 eventos em aproximadamente 9 segundos.

## Fora do escopo

`Task.WhenAll`, `SemaphoreSlim`, falhas parciais, retry, timeout, provider real e persistência real.

## Próxima sprint

Sprint 6 — Concorrência com `Task.WhenAll`.

---

# Sprint 6 em andamento — Concorrência com Task.WhenAll

## Objetivo

Executar os providers concorrentemente com `Task.WhenAll` e comparar a duração total com a linha de base sequencial de aproximadamente 9 segundos.

## Itens

- Isolar a execução de um provider em um método assíncrono que retorne seus eventos e seu `ProviderExecutionResult`.
- Criar uma task por provider sem aguardar individualmente dentro da criação da coleção.
- Aguardar todas as execuções com `Task.WhenAll`.
- Agregar os eventos somente após a conclusão das tasks, evitando escrita concorrente em `List<T>` compartilhada.
- Preservar logs, métricas individuais, `CancellationToken` e persistência única após a coleta.
- Validar que os três providers continuam retornando 6 eventos.
- Comparar a duração total concorrente com a linha de base sequencial.

## Fora do escopo

`SemaphoreSlim`, limite de concorrência, falhas parciais, `try/catch` por provider, retry, timeout, provider real e persistência real.

## Critérios de aceite

- Os três providers iniciam antes da conclusão do provider mais lento.
- `Task.WhenAll` aguarda todas as execuções.
- Nenhuma lista mutável compartilhada é escrita pelas tasks concorrentes.
- O repositório recebe 6 eventos uma única vez.
- Os resultados individuais continuam corretos.
- A duração total fica próxima do provider mais lento, aproximadamente 5 segundos.
- A solution compila sem erros ou avisos.

## Commit esperado

`refactor(fight-events): execute event providers concurrently`

## Checkpoint aprovado — preparação para concorrência

- [x] Isolar a execução individual em método assíncrono.
- [x] Retornar eventos e `ProviderExecutionResult` sem escrita compartilhada.
- [x] Preservar execução sequencial, métricas e total de 6 eventos.
- [ ] Criar uma task por provider.
- [ ] Aguardar todas com `Task.WhenAll`.
- [ ] Agregar os resultados após a conclusão das tasks.
- [ ] Validar duração total próxima de 5 segundos.

---

# Sprint 6 concluída — Concorrência com Task.WhenAll

## Entregue

- Método independente por provider.
- Uma task por provider.
- Espera conjunta com `Task.WhenAll`.
- Ausência de escrita concorrente em listas compartilhadas.
- Agregação e persistência únicas após a coleta.
- Redução da duração total de aproximadamente 9 para 5 segundos.
- Validação com três providers e 6 eventos.

## Fora do escopo

`SemaphoreSlim`, limite de concorrência, falhas parciais, retry, timeout, provider real e persistência real.

## Próxima sprint

Sprint 7 — Concorrência limitada, com limite inicial de 5 providers simultâneos.

---

# Sprint 7 concluída — Concorrência limitada

## Entregue

- Limite de concorrência centralizado em `MaximumConcurrency`.
- Uma instância compartilhada de `SemaphoreSlim` por execução do handler.
- Aquisição assíncrona e cancelável antes de cada provider.
- Liberação garantida em `finally` e descarte por escopo.
- Preservação de `Task.WhenAll`, métricas individuais, agregação segura e persistência única.

## Evidências

- Teste controlado com limite 2: o terceiro provider aguardou uma vaga e a coleta terminou em aproximadamente 6 segundos.
- Configuração definitiva com limite 5: três providers simultâneos, 6 eventos e duração total aproximada de 5 segundos.
- Build do checkpoint: 0 erros e 5 avisos preexistentes de nulabilidade em `FightEvent`.

## Fora do escopo

Falhas parciais, retry, timeout, provider real, scheduler, distributed lock e persistência real.

## Próxima sprint

Sprint 8 — Isolamento de falhas por provider.

## Commit confirmado

- `6fb7c67` — `refactor(fight-events): limit concurrent provider execution`
---

# Sprint 8 em andamento — Motor de coleta resiliente

## Objetivo

Transformar o fluxo concorrente atual em um motor de coleta capaz de preservar resultados bem-sucedidos quando um provider falha, impor timeout individual, usar configurações externas e possuir testes automatizados essenciais.

## Capacidade entregue ao final

Uma falha ou timeout em uma fonte não impede a persistência dos eventos coletados pelas demais, e o resultado da coleta informa claramente sucessos e falhas.

## Checkpoint 1 — Contrato de sucesso parcial

- [x] Definir como `ProviderExecutionResult` representa sucesso e falha.
- [x] Definir os totais derivados em `CollectEventsResponse`.
- [x] Definir o comportamento quando alguns ou todos os providers falham.
- [x] Preservar a propagação de cancelamento solicitado pelo Worker.

## Checkpoint 1 aprovado — Contrato de sucesso parcial

- `ProviderExecutionStatus` define `Succeeded`, `Failed` e `TimedOut`.
- `ProviderExecutionResult` mantém provider, quantidade, duração, status e mensagem de erro opcional.
- `IsSuccess`, `IsFailed` e `IsTimedOut` são derivados do status, evitando estados contraditórios.
- `CollectEventsResponse` calcula totais de execuções, sucessos, falhas, timeouts, eventos e `HasFailures`.
- Retorno vazio de um provider permanece sucesso; falha depende de exceção ou timeout.
- Cancelamento global não é modelado como status de provider e continuará sendo propagado.
- Handler adaptado apenas para o caminho de sucesso; isolamento de exceções permanece no Checkpoint 2.
- Build validado com 0 erros e 0 avisos.
## Checkpoint 2 — Isolamento de falhas

- [x] Capturar falhas dentro da execução individual de cada provider.
- [x] Garantir que uma falha não interrompa as demais tasks.
- [x] Persistir somente os eventos dos providers bem-sucedidos.
- [x] Registrar logs estruturados de sucesso e erro.
- [x] Manter liberação do `SemaphoreSlim` em `finally`.

## Checkpoint 2 aprovado — Isolamento de falhas

- Exceções comuns são convertidas em `ProviderExecutionResult` com status `Failed`.
- Cancelamento global solicitado pelo Worker é propagado com `throw` e não vira falha comum.
- Duração real de sucesso e falha é preservada.
- `Task.WhenAll` recebe resultados das falhas comuns sem interromper os demais providers.
- Apenas eventos de resultados bem-sucedidos são agregados.
- Repositório é chamado uma única vez e somente quando há eventos.
- `SemaphoreSlim.Release()` permanece protegido por `finally`.
- Falha parcial validada: A e C tiveram sucesso, B falhou, 3 eventos foram persistidos e o Host permaneceu ativo.
- Fluxo normal restaurado: 3 providers, 6 eventos e duração aproximada de 5 segundos.
- Build final validado com 0 erros e 0 avisos.
## Checkpoint 3 — Configuração e timeout

- Remover o limite de concorrência fixo do handler.
- Configurar limite de concorrência e timeout por provider por meio de Options.
- Aplicar timeout individual sem confundi-lo com o cancelamento global do Worker.
- Validar configurações inválidas na inicialização.

## Checkpoint 4 — Testes e fechamento

- Criar projeto de testes unitários da Application.
- Cobrir sucesso total, falha parcial, falha total, timeout e cancelamento global.
- Validar que somente eventos bem-sucedidos chegam ao repositório.
- Confirmar que a concorrência limitada continua funcionando.

## Critérios de aceite

- A falha de um provider não cancela nem descarta os resultados dos demais.
- Cancelamento do Worker continua encerrando a coleta cooperativamente.
- Timeout é registrado como falha do provider, não como cancelamento global.
- A resposta informa providers executados, sucessos, falhas e total de eventos coletados.
- O repositório recebe somente eventos de execuções bem-sucedidas e é chamado uma única vez quando houver eventos.
- Limite de concorrência e timeout são configuráveis e validados.
- Testes automatizados cobrem os cinco cenários definidos.
- Build e testes terminam sem erros.

## Fora do escopo

Retry, provider externo real, normalização, deduplicação, PostgreSQL, scheduler, distributed lock e Docker.

## Definition of Done

- Todos os critérios de aceite validados.
- Revisão técnica aprovada.
- Documentação local e Notion atualizados.
- Commit principal da sprint criado e verificado.

## Commit principal esperado

`feat(fight-events): add resilient provider collection`