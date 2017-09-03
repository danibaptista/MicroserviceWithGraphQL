using DDD.Domain.Core.SeedWork;
using System;

namespace MicroserviceArchitecture.GameOfThrones.Domain.AggregatesModel.OrderAggregate
{
    //This is just the RepositoryContracts or Interface defined at the Domain Layer
    //as requisite for the Order Aggregate

    public interface IOrderRepository : IGenericRepository<Order, Guid>
    {
    }
}