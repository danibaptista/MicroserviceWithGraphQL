using DDD.EventSourcing.Core.Events;
using DDD.Infra.EventSourcing.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Infra.EventSourcing.Core.DatabaseMappings
{
    internal class StoredEventMap : EntityTypeConfiguration<StoredEvent>
    {
        public override void Map(EntityTypeBuilder<StoredEvent> builder)
        {
            builder.Property(c => c.Timestamp)
                .HasColumnName("CreationDate");

            builder.Property(c => c.EventType)
                .HasColumnType("varchar(100)");
        }
    }
}