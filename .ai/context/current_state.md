# Estado atual

## Implementado

- Solução .NET 10 separada em `Domain`, `Application`, `Infrastructure` e `Worker`.
- Entidade `FightEvent` com organização, nome, descrição, data, local e URL de origem.
- Contratos `IEventProvider` e `IFightEventRepository` definidos na Application.
- `CollectEventsCommandHandler` percorre todos os providers registrados, agrega eventos e chama `AddRangeAsync`.
- `Worker` cria escopo e executa uma coleta no início da aplicação.
- Provider fake retorna dois eventos de exemplo; repositório fake apenas registra a quantidade recebida.

## Estado de CQRS/MediatR

`CollectEventsCommand` implementa `IRequest<CollectEventsResponse>` e MediatR está referenciado. Contudo, o handler não implementa `IRequestHandler` e o Worker resolve/invoca `CollectEventsCommandHandler` diretamente. Portanto, há intenção e dependência de MediatR, mas não um fluxo MediatR completo em execução.

## Não implementado

- Persistência real, DbContext, banco de dados e migrations.
- Providers externos reais, HTTP/scraping, normalização ou validação de dados externos.
- Deduplicação, upsert e atualização de eventos existentes.
- Execução periódica: o Worker executa uma vez; não há cron ou loop de agenda.
- Concorrência limitada, retries, timeout/política de falhas por provider e uso do `ProviderExecutionResult`.
- Testes, Docker operacional, CI/CD, métricas, tracing, health checks e contrato de integração com a API principal.

## Observações

`FakeEventProvider` contém datas de 2027 e URL `powergrind.com` apenas como dados de demonstração. Não representam eventos reais nem uma integração externa concluída.
## Atualização — Sprint 4 concluída

### Implementado

- Três implementações fake de `IEventProvider` foram registradas no DI: `FakeEventProviderA`, `FakeEventProviderB` e `FakeEventProviderC`.
- Os providers executam sequencialmente pelo `CollectEventsCommandHandler`, com delays de 1, 3 e 5 segundos e retornam, respectivamente, 2, 3 e 1 eventos.
- `IEnumerable<IEventProvider>` recebeu as três implementações; os logs identificaram cada provider individualmente e o repositório fake registrou 6 eventos persistidos.
- A solution compilou sem avisos ou erros e a execução do Worker terminou a coleta sem falhas.

### Planejado

- A Sprint 5 tratará apenas da observabilidade básica da coleta. Concorrência, timeout, retry, providers reais e persistência real permanecem fora do escopo atual.

## Atualização — Sprint 5 concluída

### Implementado

- `ProviderExecutionResult` registra o nome do provider, a quantidade de eventos coletados e a duração da execução.
- `CollectEventsResponse` passou a expor os resultados individuais e calcula `TotalCollectedEvents` a partir deles.
- `CollectEventsCommandHandler` mede e registra início, conclusão, quantidade e duração de cada provider.
- A coleta total passou a registrar quantidade de providers, total de eventos e duração.
- A execução continua sequencial e persistindo todos os eventos após a conclusão dos providers.

### Validação

- Fake Provider A: 2 eventos em aproximadamente 1 segundo.
- Fake Provider B: 3 eventos em aproximadamente 3 segundos.
- Fake Provider C: 1 evento em aproximadamente 5 segundos.
- Total: 3 providers, 6 eventos e aproximadamente 9 segundos.
- Solution compilada sem avisos ou erros.

### Planejado

- A Sprint 6 introduzirá concorrência com `Task.WhenAll`, preservando os resultados individuais e sem adicionar limitação de concorrência ou tratamento de falhas parciais.

## Atualização — Sprint 6 em andamento

- A execução individual de cada provider foi isolada em `CollectEventsFromProviderAsync`.
- O método retorna os eventos e o `ProviderExecutionResult` como uma unidade independente, sem escrever em listas compartilhadas.
- O `Handle` permanece sequencial neste checkpoint e apenas agrega os retornos.
- Validação preservada: 3 providers, 6 eventos e duração total aproximada de 9 segundos.
- O build foi concluído com sucesso; permanecem 5 avisos preexistentes de nulabilidade em `FightEvent`.
- `Task.WhenAll` ainda não foi implementado.

## Atualização — Sprint 6 concluída

### Implementado

- A execução individual foi isolada em `CollectEventsFromProviderAsync`.
- Uma task é criada para cada provider e todas são aguardadas com `Task.WhenAll`.
- Cada task retorna seus eventos e seu `ProviderExecutionResult` sem escrever em listas compartilhadas.
- Eventos e métricas são agregados somente após a conclusão das tasks.
- A persistência continua acontecendo uma única vez após a coleta.
- Logs, métricas individuais e `CancellationToken` foram preservados.

### Validação

- Os três providers iniciaram antes da conclusão do primeiro.
- Fake Provider A: 2 eventos em aproximadamente 1 segundo.
- Fake Provider B: 3 eventos em aproximadamente 3 segundos.
- Fake Provider C: 1 evento em aproximadamente 5 segundos.
- Total: 3 providers, 6 eventos e aproximadamente 5 segundos.
- Solution compilada sem erros ou avisos.

### Planejado

- A Sprint 7 introduzirá concorrência limitada com `SemaphoreSlim`, mantendo `Task.WhenAll`.

## Commit da Sprint 6

- `4e8aa9f` — `refactor(fight-events): execute event providers concurrently`.

## Atualização — Sprint 7 concluída

### Implementado

- A concorrência dos providers passou a ser limitada por uma única instância de `SemaphoreSlim` criada por execução do handler.
- `MaximumConcurrency` foi definido em 5 providers simultâneos.
- Cada provider aguarda uma vaga com `WaitAsync(cancellationToken)` antes de iniciar sua coleta.
- A permissão é devolvida em `finally`, inclusive quando a execução falha ou é cancelada depois da aquisição.
- O semáforo é descartado ao final do escopo com `using`.
- `Task.WhenAll`, a agregação posterior dos resultados e a persistência única foram preservados.

### Validação

- Com limite temporário 2, A e B iniciaram juntos; C aguardou A terminar e a duração total ficou em aproximadamente 6 segundos.
- Com o limite definitivo 5, os três providers iniciaram juntos e a duração total ficou em aproximadamente 5 segundos.
- O repositório recebeu 6 eventos em ambos os cenários.
- A validação funcional foi concluída; no build do checkpoint a solution compilou com 0 erros e 5 avisos preexistentes de nulabilidade em `FightEvent`.

### Planejado

- A Sprint 8 tratará falhas parciais, impedindo que a falha de um provider descarte os resultados dos demais.

## Commit da Sprint 7

- `6fb7c67` — `refactor(fight-events): limit concurrent provider execution`.

## Checkpoint da sessão — 2026-07-19

### Estado confirmado

- Sprint 7 implementada e registrada no commit `6fb7c67`, já presente em `master` e `origin/master`.
- Repositório estava limpo antes das atualizações documentais deste checkpoint.
- Concorrência limitada localmente a 5 providers por execução do handler.
- Kanban consolidado sem duplicidade da Sprint 7.
- Decisão permanente registrada no ADR 0005.

### Validação do checkpoint

- Build concluído com 0 erros e 5 avisos de nulabilidade em `FightEvent`.
- Não existem projetos de testes automatizados na pasta `tests`.

### Pendências e riscos

- Uma exceção em qualquer provider ainda faz `Task.WhenAll` lançar e impede o processamento dos resultados bem-sucedidos; será o foco da Sprint 8.
- `MaximumConcurrency` está fixo em código e ainda não é configurável.
- O semáforo limita apenas uma execução local; não impede coletas sobrepostas ou múltiplas instâncias do Worker.
- Os 5 avisos de nulabilidade de `FightEvent` permanecem como dívida técnica.
- Não há testes automatizados protegendo concorrência, cancelamento e agregação.

### Próxima ação recomendada

Abrir a Sprint 8 para isolamento de falhas por provider, começando por definir o comportamento esperado quando um provider falha e os demais têm sucesso.
## Sprint 8 aberta — Motor de coleta resiliente

- A Sprint 8 passa a agrupar falhas parciais, timeout, configuração e testes como uma única capacidade operacional.
- O trabalho será acompanhado por quatro checkpoints internos, sem criar uma nova sprint para cada mecanismo técnico.
- Retry foi adiado para a primeira integração externa real.
- Primeiro checkpoint: definir o contrato de sucesso parcial antes de alterar o handler.
## Sprint 8 — Checkpoint 1 aprovado

- Contrato de resultado evoluído para representar sucesso, falha e timeout.
- Propriedades booleanas e totais são calculados a partir de `ProviderExecutionStatus`.
- `CollectEventsResponse` informa total de providers, sucessos, falhas, timeouts, eventos e presença de falhas.
- Retorno de zero eventos foi definido como execução bem-sucedida.
- O handler continua implementando apenas o caminho de sucesso; falhas serão isoladas no Checkpoint 2.
- Build concluído com 0 erros e 0 avisos.

### Próxima ação

Checkpoint 2: capturar falhas por provider sem capturar o cancelamento global, manter as demais tasks e persistir apenas eventos bem-sucedidos.
## Sprint 8 — Checkpoint 2 aprovado

- Falhas comuns são isoladas por provider e representadas como `Failed`.
- Cancelamento global continua sendo propagado para encerrar a coleta cooperativamente.
- Resultados bem-sucedidos continuam sendo processados quando outra fonte falha.
- Persistência ocorre uma única vez e somente para eventos de execuções bem-sucedidas.
- Cenário parcial validado com 2 sucessos, 1 falha e 3 eventos persistidos.
- Cenário normal restaurado e validado com 3 sucessos, 6 eventos e duração aproximada de 5 segundos.
- Build concluído com 0 erros e 0 avisos.

### Próxima ação

Checkpoint 3: externalizar limite de concorrência e timeout com Options, aplicar timeout individual e validar configurações na inicialização.