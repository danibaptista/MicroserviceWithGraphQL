using DDD.EventSourcing.Core.Events;
using DDD.Infra.EventSourcing.Core.Repositories;
using Newtonsoft.Json;
using System;

namespace DDD.Infra.EventSourcing.Core.EventSourcing
{
    public sealed class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        // private readonly string _user;

        public EventStore(IEventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository ?? throw new ArgumentNullException(nameof(eventStoreRepository));
            // _user = user;
        }

        public void Save<T>(T theEvent) where T : Event
        {
            var serializedData = JsonConvert.SerializeObject(theEvent);

            var storedEvent = new StoredEvent(
                theEvent,
                serializedData,
                string.Empty);

            _eventStoreRepository.Store(storedEvent);
        }
    }
}