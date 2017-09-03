using System;

namespace DDD.EventSourcing.Core.Events
{
    public class StoredEvent : Event
    {
        public string Data { get; private set; }

        public Guid Id { get; private set; }

        public string User { get; private set; }

        public StoredEvent(Event theEvent, string data, string user)
        {
            Id = Guid.NewGuid();
            AggregateId = theEvent.AggregateId;
            EventType = theEvent.EventType;
            Data = data;
            User = user;
        }

        // EF Constructor
        protected StoredEvent() { }
    }
}