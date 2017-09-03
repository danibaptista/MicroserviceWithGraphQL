using DDD.Domain.Core.Models;
using DDD.EventSourcing.Core.Bus;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.Infrastructure
{
    internal static class IEventBusExtension
    {
        public static async Task DispatchDomainEventsAsync(this IEventBus bus, IDbContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.DomainEvents.Clear());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await bus.PublishEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}