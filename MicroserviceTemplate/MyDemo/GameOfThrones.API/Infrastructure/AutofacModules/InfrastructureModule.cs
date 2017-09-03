using Autofac;

namespace MicroserviceArchitecture.GameOfThrones.API.Infrastructure.AutofacModules
{
    using Domain.AggregatesModel.BuyerAggregate;
    using Domain.AggregatesModel.OrderAggregate;
    using DomainDrivenDesign.Infrastructure;
    using DomainDrivenDesign.Infrastructure.Idempotency;
    using GameOfThrones.Infrastructure.Repositories;
    using MicroserviceArchitecture.GameOfThrones.Infrastructure;

    public class InfrastructureModule
        : Autofac.Module
    {
        public InfrastructureModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BuyerRepository>()
                .As<IBuyerRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderRepository>()
                .As<IOrderRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestManager>()
               .As<IRequestManager>()
               .InstancePerLifetimeScope();

            builder.RegisterType<OrderingContext>()
                .As<IDbContext>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}