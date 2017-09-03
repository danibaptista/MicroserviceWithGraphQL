using DDD.Domain.Core.SeedWork;
using System;
using System.Threading.Tasks;

namespace MicroserviceArchitecture.GameOfThrones.Domain.AggregatesModel.BuyerAggregate
{
    //This is just the RepositoryContracts or Interface defined at the Domain Layer
    //as requisite for the Buyer Aggregate

    public interface IBuyerRepository : IGenericRepository<Buyer, Guid>
    {
        Task<Buyer> Find(Guid identity);
    }
}