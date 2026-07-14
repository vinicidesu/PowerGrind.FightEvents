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