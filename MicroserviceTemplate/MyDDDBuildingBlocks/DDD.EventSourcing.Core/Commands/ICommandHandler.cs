using DDD.EventSourcing.Core.Commands;
using MediatR;

namespace DDD.EventSourcing.Core.Events
{
    public interface ICommandHandler<in TCommand, TCommandResponse> : IAsyncRequestHandler<TCommand, TCommandResponse>
       where TCommand : Command<TCommandResponse>
       where TCommandResponse : CommandResponse
    {
    }
}