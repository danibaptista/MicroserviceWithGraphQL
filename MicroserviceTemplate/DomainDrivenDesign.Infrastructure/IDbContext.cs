using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Infrastructure
{
    using DDD.Domain.Core.SeedWork;

    public interface IDbContext : IUnitOfWork
    {
        // Summary: Provides access to information and operations for entity instances this context
        // is tracking.
        ChangeTracker ChangeTracker { get; }

        DbContext DbContext { get; }

        // Summary: Begins tracking the given entity, and any other reachable entities that are not
        // already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added state such
        // that they will be inserted into the database when
        // Microsoft.EntityFrameworkCore.DbContext.SaveChanges is called.
        //
        // Parameters: entity: The entity to add.
        //
        // Returns: The Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry for the entity. The
        // entry provides access to change tracking information and operations for the entity.
        EntityEntry Add(object entity);

        // Summary: Finds an entity with the given primary key values. If an entity with the given
        // primary key values is being tracked by the context, then it is returned immediately
        // without making a request to the database. Otherwise, a query is made to the database for
        // an entity with the given primary key values and this entity, if found, is attached to the
        // context and returned. If no entity is found, then null is returned.
        //
        // Parameters: keyValues: The values of the primary key for the entity to be found.
        //
        // Type parameters: TEntity: The type of entity to find.
        //
        // Returns: The entity found, or null.
        TEntity Find<TEntity>(params object[] keyValues) where TEntity : class;

        // Summary: Finds an entity with the given primary key values. If an entity with the given
        // primary key values is being tracked by the context, then it is returned immediately
        // without making a request to the database. Otherwise, a query is made to the database for
        // an entity with the given primary key values and this entity, if found, is attached to the
        // context and returned. If no entity is found, then null is returned.
        //
        // Parameters: entityType: The type of entity to find.
        //
        // keyValues: The values of the primary key for the entity to be found.
        //
        // Returns: The entity found, or null.
        object Find(Type entityType, params object[] keyValues);

        // Summary: Finds an entity with the given primary key values. If an entity with the given
        // primary key values is being tracked by the context, then it is returned immediately
        // without making a request to the database. Otherwise, a query is made to the database for
        // an entity with the given primary key values and this entity, if found, is attached to the
        // context and returned. If no entity is found, then null is returned.
        //
        // Parameters: keyValues: The values of the primary key for the entity to be found.
        //
        // Type parameters: TEntity: The type of entity to find.
        //
        // Returns: The entity found, or null.
        Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class;

        // Summary: Finds an entity with the given primary key values. If an entity with the given
        // primary key values is being tracked by the context, then it is returned immediately
        // without making a request to the database. Otherwise, a query is made to the database for
        // an entity with the given primary key values and this entity, if found, is attached to the
        // context and returned. If no entity is found, then null is returned.
        //
        // Parameters: entityType: The type of entity to find.
        //
        // keyValues: The values of the primary key for the entity to be found.
        //
        // Returns: The entity found, or null.
        Task<object> FindAsync(Type entityType, params object[] keyValues);

        // Summary: Finds an entity with the given primary key values. If an entity with the given
        // primary key values is being tracked by the context, then it is returned immediately
        // without making a request to the database. Otherwise, a query is made to the database for
        // an entity with the given primary key values and this entity, if found, is attached to the
        // context and returned. If no entity is found, then null is returned.
        //
        // Parameters: keyValues: The values of the primary key for the entity to be found.
        //
        // cancellationToken: A System.Threading.CancellationToken to observe while waiting for the
        // task to complete.
        //
        // Type parameters: TEntity: The type of entity to find.
        //
        // Returns: The entity found, or null.
        Task<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken) where TEntity : class;

        // Summary: Finds an entity with the given primary key values. If an entity with the given
        // primary key values is being tracked by the context, then it is returned immediately
        // without making a request to the database. Otherwise, a query is made to the database for
        // an entity with the given primary key values and this entity, if found, is attached to the
        // context and returned. If no entity is found, then null is returned.
        //
        // Parameters: entityType: The type of entity to find.
        //
        // keyValues: The values of the primary key for the entity to be found.
        //
        // cancellationToken: A System.Threading.CancellationToken to observe while waiting for the
        // task to complete.
        //
        // Returns: The entity found, or null.
        Task<object> FindAsync(Type entityType, object[] keyValues, CancellationToken cancellationToken);

        // Summary: Creates a Microsoft.EntityFrameworkCore.DbSet`1 that can be used to query and
        // save instances of TEntity.
        //
        // Type parameters: TEntity: The type of entity for which a set should be returned.
        //
        // Returns: A set for the given entity type.
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}