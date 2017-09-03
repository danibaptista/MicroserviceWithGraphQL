using DDD.EventSourcing.Core.Bus;
using DDD.EventSourcing.Core.Events;
using DDD.Infra.EventSourcing.Core.DatabaseMappings;
using DDD.Infra.EventSourcing.Core.Extensions;
using DDD.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DDD.Infra.EventSourcing.Core.Contexts
{
    public sealed class EventStoreContext : BaseDbContext
    {
        public DbSet<StoredEvent> StoredEvent { get; set; }

        public EventStoreContext(DbContextOptions options, IEventBus mediator, string defaultSchema) : base(options, mediator, defaultSchema)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddConfiguration(new StoredEventMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}