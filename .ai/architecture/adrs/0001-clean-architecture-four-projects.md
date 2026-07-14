# ADR 0001 — Clean Architecture em quatro projetos

## Contexto

A solução contém projetos Domain, Application, Infrastructure e Worker, com referências direcionadas para dentro.

## Decisão

Manter regras/modelos no Domain, casos de uso e abstrações na Application, adaptadores concretos na Infrastructure e composição/execução no Worker.

## Consequências

Providers e persistência podem evoluir sem acoplar o caso de uso a HTTP ou banco. Há custo de navegação entre projetos, aceitável para preservar limites enquanto o contexto cresce.

## Trade-offs

Para o escopo atual, algumas camadas possuem pouco código. A estrutura é justificada pela intenção de múltiplas fontes e persistência futura, mas novas abstrações devem surgir por necessidade concreta.
