# Backlog

- [ ] Como academia, quero visualizar eventos de luta confiáveis para planejar a participação de atletas.
  - Aceite: eventos têm fonte, nome, data e local normalizados; regras de qualidade são definidas.
- [ ] Como operação, quero persistir eventos sem duplicá-los para manter um calendário consistente.
  - Aceite: existe schema/migration, identificador ou regra de unicidade, upsert e testes de idempotência.
- [ ] Como operação, quero que a coleta seja periódica e resiliente para manter dados atualizados.
  - Aceite: agenda configurável, timeout, retry, cancelamento e falha isolada por provider.
- [ ] Como equipe, quero monitorar cada fonte para detectar coleta degradada.
  - Aceite: logs estruturados e métricas registram duração, quantidade e erro por provider.
- [ ] Como integrador, quero um contrato para consumir eventos coletados no PowerGrind.
  - Aceite: contrato, propriedade dos dados e forma de sincronização são definidos com a API principal.

- [ ] Como academia e treinador, quero relacionar eventos confiáveis ao planejamento de treinos e participação de alunos.
  - Aceite: a API principal define o público, as modalidades e o contrato de consumo antes de qualquer integração.
