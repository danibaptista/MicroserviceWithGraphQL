using MediatR;
using System;

namespace DDD.EventSourcing.Core.Events
{
    public abstract class Event : INotification
    {
        public Guid AggregateId { get; protected set; }
        public string EventType { get; protected set; }
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            EventType = GetType().Name;
            Timestamp = DateTime.Now;
        }
    }
}