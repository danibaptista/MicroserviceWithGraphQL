using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace MicroserviceArchitecture.GameOfThrones.Infrastructure.Repositories
{
    using DDD.Infrastructure.Repositories;
    using Domain.AggregatesModel.BuyerAggregate;
    using System;

    public class BuyerRepository
        : BaseRepository<Buyer, Guid>, IBuyerRepository
    {
        public BuyerRepository(OrderingContext context, ILogger<BuyerRepository> logger) : base(context, logger)
        {
        }

        public async Task<Buyer> Find(Guid identity)
        {
            var buyer = await dbContext.Set<Buyer>()
                .Include(b => b.PaymentMethods)
                .Where(b => b.IdentityGuid == identity)
                .SingleOrDefaultAsync();

            return buyer;
        }
    }
}