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