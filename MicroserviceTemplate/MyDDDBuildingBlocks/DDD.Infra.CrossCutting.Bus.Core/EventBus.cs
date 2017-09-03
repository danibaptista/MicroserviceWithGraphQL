using MediatR;
using System.Threading.Tasks;

namespace DDD.Infra.CrossCutting.Bus.Core
{
    using DDD.EventSourcing.Core.Bus;
    using DDD.EventSourcing.Core.Commands;
    using DDD.EventSourcing.Core.Events;
    using DDD.EventSourcing.Core.Notifications;

    public sealed class InMemoryBus : IEventBus
    {
        private readonly IEventStore _eventStore;
        private readonly IMediator _mediator;

        public InMemoryBus(IEventStore eventStore, IMediator mediator)
        {
            _eventStore = eventStore;
            _mediator = mediator;
        }

        public Task PublishEvent<T>(T @event) where T : Event
        {
            _eventStore?.Save(@event);

            return _mediator.Publish(@event);
        }

        public Task RaiseNotification<T>(T notification) where T : DomainNotification
        {
            return _mediator.Publish(notification);
        }

        public Task<TResponse> SendCommand<TResponse>(Command<TResponse> command) where TResponse : CommandResponse
        {
            return _mediator.Send(command);
        }
    }
}