using System.Threading.Tasks;

namespace DDD.EventSourcing.Core.Bus
{
    using Commands;
    using Events;
    using Notifications;

    public interface IEventBus
    {
        Task PublishEvent<T>(T @event) where T : Event;

        Task RaiseNotification<T>(T notification) where T : DomainNotification;

        Task<TResponse> SendCommand<TResponse>(Command<TResponse> command)
            where TResponse : CommandResponse;
    }
}