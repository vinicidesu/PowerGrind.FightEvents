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
