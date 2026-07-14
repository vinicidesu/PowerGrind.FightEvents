# ADR 0004 — MediatR presente, mas dispatch direto vigente

## Contexto

MediatR é uma dependência da Application e `CollectEventsCommand` implementa `IRequest<CollectEventsResponse>`. O Worker, porém, resolve o handler diretamente e o handler não implementa `IRequestHandler`.

## Decisão

Documentar dispatch direto como comportamento vigente. Não afirmar CQRS/MediatR completo até que o handler seja registrado/implementado como handler MediatR e o Worker envie o comando via mediator.

## Consequências

O projeto não deve criar abstrações adicionais de comandos/queries apenas por convenção. A decisão de ativar MediatR de ponta a ponta fica pendente e deve ser motivada por casos de uso reais.

## Trade-offs

O dispatch direto é simples e explícito. Usar MediatR posteriormente pode padronizar múltiplos casos de uso e behaviors, mas adiciona indireção e configuração.
