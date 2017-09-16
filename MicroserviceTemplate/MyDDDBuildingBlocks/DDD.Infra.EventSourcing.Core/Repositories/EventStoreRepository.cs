using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Infra.EventSourcing.Core.Repositories
{
    using DDD.EventSourcing.Core.Events;
    using DDD.Infra.EventSourcing.Core.Contexts;

    public sealed class EventStoreRepository : IEventStoreRepository
    {
        private readonly EventStoreContext _context;

        public EventStoreRepository(EventStoreContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<StoredEvent> GetAll(Guid aggregateId)
        {
            return _context.StoredEvent.Where(e => e.AggregateId == aggregateId);
        }

        public void Store(StoredEvent theEvent)
        {
            _context.StoredEvent.Add(theEvent);
            _context.SaveChanges();
        }
    }
}