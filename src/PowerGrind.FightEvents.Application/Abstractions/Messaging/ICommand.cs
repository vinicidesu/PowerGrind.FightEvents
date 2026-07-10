using MediatR;

namespace PowerGrind.FightEvents.Application.Abstractions.Messaging
{
    public interface ICommand<TResponse> : IRequest<TResponse>;
}
