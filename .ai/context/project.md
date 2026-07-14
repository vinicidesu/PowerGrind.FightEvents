# Projeto: PowerGrind.FightEvents

## Visão

`PowerGrind.FightEvents` é o contexto responsável por coletar e, futuramente, manter dados confiáveis de eventos de luta para o ecossistema PowerGrind. A intenção de produto, recuperada do histórico, é atender um SaaS de academias com calendário de competições; a integração de consumo pela API principal ainda não foi definida ou implementada.

## Stack

| Área | Implementação observada |
|---|---|
| Runtime | .NET 10 Worker Service |
| Arquitetura | Domain, Application, Infrastructure e Worker |
| Comunicação de caso de uso | MediatR 14 referenciado na Application; handler é chamado diretamente no código atual |
| Host/DI | Generic Host e `Microsoft.Extensions.DependencyInjection` |
| Observabilidade | `ILogger` padrão |
| Persistência atual | Repositório fake; nenhum banco ou migration |

## Módulos

- `Domain`: entidade `FightEvent`.
- `Application`: abstrações de provider/persistência e feature `Events/Collect`.
- `Infrastructure`: `FakeEventProvider` e `FakeFightEventRepository`.
- `Worker`: composição de DI e execução do handler de coleta.

## Princípios vigentes

O projeto usa limites de Clean Architecture e organização por feature para o caso de uso de coleta. A abordagem é incremental: primeiro validar o fluxo vertical com adaptadores falsos, depois introduzir fontes reais, persistência, políticas de falha e otimizações conforme necessário.

## Contexto ampliado de produto

O serviço atende a visão de um SaaS para academias de luta, com alunos, treinadores, treinos, evolução e check-ins. Eventos de luta serão uma capacidade complementar; o público consumidor, modalidades cobertas e contrato de integração ainda precisam ser definidos. Consulte `context/product_vision.md`.
