using MediatR;

namespace PowerGrind.FightEvents.Application.Abstractions.Messaging
{
    public interface IQuery<T> : IRequest<T>;
}
