using DDD.EventSourcing.Core.Events;
using DDD.Infra.EventSourcing.Core.Repositories;
using Newtonsoft.Json;

namespace DDD.Infra.EventSourcing.Core.EventSourcing
{
    public sealed class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly string _user;

        public EventStore(IEventStoreRepository eventStoreRepository, string user)
        {
            _eventStoreRepository = eventStoreRepository;
            _user = user;
        }

        public void Save<T>(T theEvent) where T : Event
        {
            var serializedData = JsonConvert.SerializeObject(theEvent);

            var storedEvent = new StoredEvent(
                theEvent,
                serializedData,
                _user);

            _eventStoreRepository.Store(storedEvent);
        }
    }
}