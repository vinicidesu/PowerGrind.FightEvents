# ADR 0002 — Contratos de provider e persistência na Application

## Contexto

`IEventProvider` e `IFightEventRepository` são consumidos pelo caso de uso `CollectEvents`; seus adaptadores vivem na Infrastructure.

## Decisão

Definir esses contratos na Application e implementar `FakeEventProvider`/`FakeFightEventRepository` na Infrastructure.

## Consequências

O handler não conhece uma fonte ou banco específicos. O Worker escolhe as implementações por DI.

## Trade-offs

Os contratos ainda são pequenos e podem precisar evoluir para persistência real, normalização e idempotência. Não antecipar interfaces adicionais sem um caso de uso que as exija.
