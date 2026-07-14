# Convenções observadas

- Projetos usam namespaces `PowerGrind.FightEvents.*`.
- Domain contém a entidade `FightEvent`; Application contém abstrações e features.
- Infrastructure implementa adaptadores; Worker registra dependências e dispara o caso de uso.
- Métodos assíncronos recebem `CancellationToken`.
- Logs usam `ILogger<T>`.
- A feature atual usa records para command/response e classe para o handler.
