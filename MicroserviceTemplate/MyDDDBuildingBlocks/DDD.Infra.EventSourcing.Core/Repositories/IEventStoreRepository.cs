using System;
using System.Collections.Generic;

namespace DDD.Infra.EventSourcing.Core.Repositories
{
    using DDD.EventSourcing.Core.Events;

    public interface IEventStoreRepository : IDisposable
    {
        IEnumerable<StoredEvent> GetAll(Guid aggregateId);

        void Store(StoredEvent theEvent);
    }
}