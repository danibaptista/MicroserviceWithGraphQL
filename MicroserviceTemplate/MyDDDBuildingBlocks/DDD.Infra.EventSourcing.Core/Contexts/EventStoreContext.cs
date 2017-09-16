using DDD.Domain.Core.SeedWork;
using DDD.EventSourcing.Core.Commands;
using DDD.EventSourcing.Core.Events;
using DDD.Infra.EventSourcing.Core.DatabaseMappings;
using DDD.Infra.EventSourcing.Core.Extensions;
using DDD.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Infra.EventSourcing.Core.Contexts
{
    public sealed class EventStoreContext : DbContext, IDbContext, IUnitOfWork
    {
        public DbContext DbContext { get { return this; } }
        public DbSet<StoredEvent> StoredEvent { get; set; }

        public EventStoreContext() : base()
        {
            // Database.EnsureCreated();
        }

        public async Task<CommandResponse> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // After executing this line all the changes (from the Command Handler and Domain Event
            // Handlers) performed throught the DbContext will be commited
            var result = await SaveChangesAsync();

            return new CommandResponse(result > 0);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddConfiguration(new StoredEventMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}