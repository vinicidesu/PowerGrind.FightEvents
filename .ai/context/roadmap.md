# Roadmap inicial

## Curto prazo

- Criar testes para o handler e adaptadores fake existentes.
- Definir uma primeira fonte autorizada de eventos e seu contrato de dados.
- Definir modelo de persistência, chave de identidade/deduplicação e comportamento de upsert.
- Implementar um provider real e persistência real antes de expandir fontes.

## Médio prazo

- Introduzir normalização, validação e deduplicação entre fontes.
- Executar coleta periodicamente com configuração de cadência e cancelamento correto.
- Implementar isolamento de falhas, timeout, retry e resultado por provider.
- Criar logs estruturados, métricas e health checks.

## Longo prazo

- Integrar eventos tratados ao produto PowerGrind por contrato explícito.
- Adicionar fontes conforme qualidade, termos de uso e custo operacional.
- Avaliar mensageria ou paralelismo limitado somente quando volume/cadência justificarem.