using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DDD.Infrastructure.Repositories
{
    using DDD.Domain.Core.SeedWork;

    public abstract class BaseRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot, IEntity<TKey>
    {
        protected readonly ILogger _logger;
        protected readonly IDbContext dbContext;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return dbContext;
            }
        }

        protected BaseRepository()
        {
        }

        protected BaseRepository(IDbContext db, ILogger logger)
        {
            dbContext = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual TEntity Add(TEntity entity)
        {
            _logger.LogInformation("Update {type} with id = {id}", typeof(TEntity).Name, entity.Id);
            return dbContext.Set<TEntity>().Add(entity).Entity;
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            dbContext.Set<TEntity>().AddRange(entities);
        }

        public virtual void Delete(TKey id)
        {
            _logger.LogInformation("Delete {type} with id = {id}", typeof(TEntity).Name, id);
            dbContext.Set<TEntity>().Remove(dbContext.Set<TEntity>().Find(id));
        }

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            _logger.LogInformation("Find all {type} where predicate = {predicate})", typeof(TEntity).Name, predicate);
            return dbContext.Set<TEntity>().Where(predicate);
        }

        public virtual TEntity Get(TKey id)
        {
            _logger.LogInformation("Get {type} with id = {id}", typeof(TEntity).Name, id);
            return dbContext.Set<TEntity>().SingleOrDefault(c => c.Id.Equals(id));
        }

        public virtual TEntity Get(TKey id, string include)
        {
            _logger.LogInformation("Get {type} with id = {id} (including {include})", typeof(TEntity).Name, id, include);
            return dbContext.Set<TEntity>().Include(include).SingleOrDefault(c => c.Id.Equals(id));
        }

        public virtual TEntity Get(TKey id, IEnumerable<string> includes)
        {
            _logger.LogInformation("Get {type} with id = {id} (including [{include}])", typeof(TEntity).Name, id, string.Join(",", includes));
            var query = dbContext.Set<TEntity>().AsQueryable();
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.SingleOrDefault(c => c.Id.Equals(id));
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            _logger.LogInformation("Get all {type}s", typeof(TEntity).Name);
            return dbContext.Set<TEntity>().AsQueryable();
        }

        public virtual IQueryable<TEntity> GetAll(string include)
        {
            _logger.LogInformation("Get all {type}s (including {include})", typeof(TEntity).Name, include);
            return dbContext.Set<TEntity>().Include(include).AsQueryable();
        }

        public virtual IQueryable<TEntity> GetAll(IEnumerable<string> includes)
        {
            _logger.LogInformation("Get all {type}s (including [{includes}])", typeof(TEntity).Name, string.Join(",", includes));
            var query = dbContext.Set<TEntity>().AsQueryable();
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }

        public virtual Task<List<TEntity>> GetAllAsync()
        {
            _logger.LogInformation("Get all {type}s", typeof(TEntity).Name);
            return dbContext.Set<TEntity>().ToListAsync();
        }

        public virtual Task<List<TEntity>> GetAllAsync(string include)
        {
            _logger.LogInformation("Get all {type}s (including {include})", typeof(TEntity).Name, include);
            return dbContext.Set<TEntity>().Include(include).ToListAsync();
        }

        public virtual Task<List<TEntity>> GetAllAsync(IEnumerable<string> includes)
        {
            _logger.LogInformation("Get all {type}s (including [{includes}])", typeof(TEntity).Name, string.Join(",", includes));
            var query = dbContext.Set<TEntity>().AsQueryable();
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.ToListAsync();
        }

        public virtual Task<TEntity> GetAsync(TKey id)
        {
            _logger.LogInformation("Get {type} with id = {id}", typeof(TEntity).Name, id);
            return dbContext.Set<TEntity>().SingleOrDefaultAsync(c => c.Id.Equals(id));
        }

        public virtual Task<TEntity> GetAsync(TKey id, string include)
        {
            _logger.LogInformation("Get {type} with id = {id} (including {include})", typeof(TEntity).Name, id, include);
            return dbContext.Set<TEntity>().Include(include).SingleOrDefaultAsync(c => c.Id.Equals(id));
        }

        public virtual Task<TEntity> GetAsync(TKey id, IEnumerable<string> includes)
        {
            _logger.LogInformation("Get {type} with id = {id} (including [{include}])", typeof(TEntity).Name, id, string.Join(",", includes));
            var query = dbContext.Set<TEntity>().AsQueryable();
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.SingleOrDefaultAsync(c => c.Id.Equals(id));
        }

        public virtual TEntity Post(TEntity entity)
        {
            return Equals(entity.Id, default(TKey)) ? Add(entity) : Update(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            _logger.LogInformation("Update {type} with id = {id}", typeof(TEntity).Name, entity.Id);
            return dbContext.Set<TEntity>().Update(entity).Entity;
        }
    }
}