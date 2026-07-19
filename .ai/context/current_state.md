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
