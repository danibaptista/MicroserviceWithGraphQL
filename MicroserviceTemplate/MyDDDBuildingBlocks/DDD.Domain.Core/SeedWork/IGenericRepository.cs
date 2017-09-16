using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DDD.Domain.Core.SeedWork
{
    public interface IGenericRepository<TEntity, in TKey> : IRepository<TEntity>
        where TEntity : IAggregateRoot
    {
        TEntity Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Delete(TKey id);

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        TEntity Get(TKey id);

        TEntity Get(TKey id, string include);

        TEntity Get(TKey id, IEnumerable<string> includes);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(string include);

        IQueryable<TEntity> GetAll(IEnumerable<string> includes);

        Task<List<TEntity>> GetAllAsync();

        Task<List<TEntity>> GetAllAsync(string include);

        Task<List<TEntity>> GetAllAsync(IEnumerable<string> includes);

        Task<TEntity> GetAsync(TKey id);

        Task<TEntity> GetAsync(TKey id, string include);

        Task<TEntity> GetAsync(TKey id, IEnumerable<string> includes);

        TEntity Post(TEntity entity);

        TEntity Update(TEntity entity);
    }
}