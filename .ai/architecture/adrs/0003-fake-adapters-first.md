# ADR 0003 — Adaptadores falsos antes de integrações reais

## Contexto

O projeto possui `FakeEventProvider`, com dois eventos demonstrativos, e `FakeFightEventRepository`, que registra a quantidade recebida.

## Decisão

Validar primeiro o fluxo vertical Worker → Handler → Provider → Repository com adaptadores falsos.

## Consequências

O fluxo de composição e coleta pode ser exercitado sem depender de API externa, scraping ou banco. A ausência de persistência real e de falhas de rede precisa permanecer visível no backlog.

## Trade-offs

Dados fake não validam a qualidade dos contratos de fontes reais. A primeira integração externa deve provocar a revisão de normalização, identificação e políticas de falha.
