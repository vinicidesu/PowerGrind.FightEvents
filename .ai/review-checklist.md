# Checklist de review

- [ ] Application não depende de detalhes de HTTP, banco ou provider concreto.
- [ ] Worker apenas compõe/dispara; não absorve regras de negócio.
- [ ] Providers respeitam cancelamento e isolam dados externos.
- [ ] Persistência possui regra clara de identidade/idempotência quando for real.
- [ ] Falha de uma fonte não impede coleta das demais quando essa política for implementada.
- [ ] Logs não expõem segredos e identificam provider/execução.
- [ ] Mudanças no handler e adaptadores possuem testes.
