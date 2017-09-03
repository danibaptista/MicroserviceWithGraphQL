using Microsoft.EntityFrameworkCore;

namespace DDD.Infra.EventSourcing.Core.Extensions
{
    internal static class ModelBuilderExtensions
    {
        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, EntityTypeConfiguration<TEntity> configuration) where TEntity : class
        {
            configuration.Map(modelBuilder.Entity<TEntity>());
        }
    }
}