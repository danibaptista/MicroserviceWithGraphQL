using MediatR;

namespace DDD.EventSourcing.Core.Events
{
    public interface IEventHandler<in TEvent> : IAsyncNotificationHandler<TEvent> where TEvent : Event
    {
    }
}