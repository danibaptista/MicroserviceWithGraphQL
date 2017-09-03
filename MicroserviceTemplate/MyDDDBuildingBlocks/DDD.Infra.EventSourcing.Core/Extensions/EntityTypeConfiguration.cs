using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Infra.EventSourcing.Core.Extensions
{
    internal abstract class EntityTypeConfiguration<TEntity> where TEntity : class
    {
        public abstract void Map(EntityTypeBuilder<TEntity> builder);
    }
}