using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Infrastructure.DbContexts
{
    using DDD.Domain.Core.SeedWork;
    using DDD.EventSourcing.Core.Bus;
    using DDD.EventSourcing.Core.Commands;
    using DDD.Infrastructure;
    using DDD.Infrastructure.Idempotency;

    public abstract class BaseDbContext : DbContext, IDbContext, IUnitOfWork
    {
        protected string DEFAULT_SCHEMA;
        private readonly IEventBus _bus;
        public DbContext DbContext { get { return this; } }

        public BaseDbContext(DbContextOptions options, IEventBus bus, string defaultSchema) : base(options)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            DEFAULT_SCHEMA = string.IsNullOrWhiteSpace(defaultSchema) ? throw new ArgumentNullException(nameof(defaultSchema)) : defaultSchema;
            System.Diagnostics.Debug.WriteLine("BaseDbContext::ctor ->" + this.GetHashCode());
        }

        public async Task<CommandResponse> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single
            //    transaction including side effects from the domain event handlers which are using
            // the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions.
            // You will need to handle eventual consistency and compensatory actions in case of
            // failures in any of the Handlers.
            await _bus.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event
            // Handlers) performed throught the DbContext will be commited
            var result = await SaveChangesAsync();

            return new CommandResponse(result > 0);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClientRequest>(ConfigureRequests);
        }

        private void ConfigureRequests(EntityTypeBuilder<ClientRequest> requestConfiguration)
        {
            requestConfiguration.ToTable("requests", DEFAULT_SCHEMA);
            requestConfiguration.HasKey(cr => cr.Id);
            requestConfiguration.Property(cr => cr.Name).IsRequired();
            requestConfiguration.Property(cr => cr.Time).IsRequired();
        }
    }
}