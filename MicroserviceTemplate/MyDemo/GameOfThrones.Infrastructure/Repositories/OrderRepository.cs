using Microsoft.Extensions.Logging;

namespace MicroserviceArchitecture.GameOfThrones.Infrastructure.Repositories
{
    using DDD.Infrastructure;
    using DDD.Infrastructure.Repositories;
    using Domain.AggregatesModel.OrderAggregate;
    using System;

    public class OrderRepository
        : BaseRepository<Order, Guid, IDbContext>, IOrderRepository
    {
        public OrderRepository(OrderingContext context, ILogger<OrderRepository> logger) : base(context, logger)
        {
        }
    }
}