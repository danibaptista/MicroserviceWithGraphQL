using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace MicroserviceArchitecture.GameOfThrones.Infrastructure
{
    using DDD.EventSourcing.Core.Bus;
    using DDD.Infrastructure.DbContexts;
    using Domain.AggregatesModel.BuyerAggregate;
    using Domain.AggregatesModel.OrderAggregate;

    public class OrderingContext
      : BaseDbContext

    {
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<CardType> CardTypes { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<PaymentMethod> Payments { get; set; }

        public OrderingContext(DbContextOptions options, IEventBus mediator) : base(options, mediator, "ordering")
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>(ConfigureAddress);
            modelBuilder.Entity<PaymentMethod>(ConfigurePayment);
            modelBuilder.Entity<Order>(ConfigureOrder);
            modelBuilder.Entity<OrderItem>(ConfigureOrderItems);
            modelBuilder.Entity<CardType>(ConfigureCardTypes);
            modelBuilder.Entity<OrderStatus>(ConfigureOrderStatus);
            modelBuilder.Entity<Buyer>(ConfigureBuyer);
        }

        private void ConfigureAddress(EntityTypeBuilder<Address> addressConfiguration)
        {
            addressConfiguration.ToTable("address", DEFAULT_SCHEMA);

            // DDD Pattern comment: Implementing the Address Id as "Shadow property" becuase the
            // Address is a Value-Object (VO) and an Id (Identity) is not desired for a VO EF Core
            // just needs the Id so it is capable to store it in a database table
            // See: https://docs.microsoft.com/en-us/ef/core/modeling/shadow-properties
            addressConfiguration.Property<int>("Id")
                .IsRequired();

            addressConfiguration.HasKey("Id");
        }

        private void ConfigureBuyer(EntityTypeBuilder<Buyer> buyerConfiguration)
        {
            buyerConfiguration.ToTable("buyers", DEFAULT_SCHEMA);

            buyerConfiguration.HasKey(b => b.Id);

            buyerConfiguration.Ignore(b => b.DomainEvents);

            buyerConfiguration.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo("buyerseq", DEFAULT_SCHEMA);

            buyerConfiguration.Property(b => b.IdentityGuid)
                .HasMaxLength(200)
                .IsRequired();

            buyerConfiguration.HasIndex("IdentityGuid")
              .IsUnique(true);

            buyerConfiguration.HasMany(b => b.PaymentMethods)
               .WithOne()
               .HasForeignKey("BuyerId")
               .OnDelete(DeleteBehavior.Cascade);

            var navigation = buyerConfiguration.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureCardTypes(EntityTypeBuilder<CardType> cardTypesConfiguration)
        {
            cardTypesConfiguration.ToTable("cardtypes", DEFAULT_SCHEMA);

            cardTypesConfiguration.HasKey(ct => ct.Id);

            cardTypesConfiguration.Property(ct => ct.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            cardTypesConfiguration.Property(ct => ct.Name)
                .HasMaxLength(200)
                .IsRequired();
        }

        private void ConfigureOrder(EntityTypeBuilder<Order> orderConfiguration)
        {
            orderConfiguration.ToTable("orders", DEFAULT_SCHEMA);

            orderConfiguration.HasKey(o => o.Id);

            orderConfiguration.Ignore(b => b.DomainEvents);

            orderConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("orderseq", DEFAULT_SCHEMA);

            orderConfiguration.Property<DateTime>("OrderDate").IsRequired();
            orderConfiguration.Property<int?>("BuyerId").IsRequired(false);
            orderConfiguration.Property<int>("OrderStatusId").IsRequired();
            orderConfiguration.Property<int?>("PaymentMethodId").IsRequired(false);
            orderConfiguration.Property<string>("Description").IsRequired(false);

            var navigation = orderConfiguration.Metadata.FindNavigation(nameof(Order.OrderItems));
            // DDD Patterns comment:
            //Set as Field (New since EF 1.1) to access the OrderItem collection property through its field
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            orderConfiguration.HasOne<PaymentMethod>()
                .WithMany()
                .HasForeignKey("PaymentMethodId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            orderConfiguration.HasOne<Buyer>()
                .WithMany()
                .IsRequired(false)
                .HasForeignKey("BuyerId");

            orderConfiguration.HasOne(o => o.OrderStatus)
                .WithMany()
                .HasForeignKey("OrderStatusId");
        }

        private void ConfigureOrderItems(EntityTypeBuilder<OrderItem> orderItemConfiguration)
        {
            orderItemConfiguration.ToTable("orderItems", DEFAULT_SCHEMA);

            orderItemConfiguration.HasKey(o => o.Id);

            orderItemConfiguration.Ignore(b => b.DomainEvents);

            orderItemConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("orderitemseq");

            orderItemConfiguration.Property<int>("OrderId")
                .IsRequired();

            orderItemConfiguration.Property<decimal>("Discount")
                .IsRequired();

            orderItemConfiguration.Property<int>("ProductId")
                .IsRequired();

            orderItemConfiguration.Property<string>("ProductName")
                .IsRequired();

            orderItemConfiguration.Property<decimal>("UnitPrice")
                .IsRequired();

            orderItemConfiguration.Property<int>("Units")
                .IsRequired();

            orderItemConfiguration.Property<string>("PictureUrl")
                .IsRequired(false);
        }

        private void ConfigureOrderStatus(EntityTypeBuilder<OrderStatus> orderStatusConfiguration)
        {
            orderStatusConfiguration.ToTable("orderstatus", DEFAULT_SCHEMA);

            orderStatusConfiguration.HasKey(o => o.Id);

            orderStatusConfiguration.Property(o => o.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            orderStatusConfiguration.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();
        }

        private void ConfigurePayment(EntityTypeBuilder<PaymentMethod> paymentConfiguration)
        {
            paymentConfiguration.ToTable("paymentmethods", DEFAULT_SCHEMA);

            paymentConfiguration.HasKey(b => b.Id);

            paymentConfiguration.Ignore(b => b.DomainEvents);

            paymentConfiguration.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo("paymentseq", DEFAULT_SCHEMA);

            paymentConfiguration.Property<int>("BuyerId")
                .IsRequired();

            paymentConfiguration.Property<string>("CardHolderName")
                .HasMaxLength(200)
                .IsRequired();

            paymentConfiguration.Property<string>("Alias")
                .HasMaxLength(200)
                .IsRequired();

            paymentConfiguration.Property<string>("CardNumber")
                .HasMaxLength(25)
                .IsRequired();

            paymentConfiguration.Property<DateTime>("Expiration")
                .IsRequired();

            paymentConfiguration.Property<int>("CardTypeId")
                .IsRequired();

            paymentConfiguration.HasOne(p => p.CardType)
                .WithMany()
                .HasForeignKey("CardTypeId");
        }
    }
}