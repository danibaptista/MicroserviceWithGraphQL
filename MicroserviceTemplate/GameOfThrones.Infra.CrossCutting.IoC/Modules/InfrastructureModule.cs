using Autofac;
using DDD.EventSourcing.Core.Bus;
using DDD.EventSourcing.Core.Events;
using DDD.Infra.CrossCutting.Bus.Core;
using DDD.Infra.EventSourcing.Core.Contexts;
using DDD.Infra.EventSourcing.Core.EventSourcing;
using DDD.Infra.EventSourcing.Core.Repositories;
using DDD.Infrastructure;
using DDD.Infrastructure.Idempotency;
using MediatR;
using MicroserviceArchitecture.GameOfThrones.Domain.AggregatesModel.BuyerAggregate;
using MicroserviceArchitecture.GameOfThrones.Domain.AggregatesModel.OrderAggregate;
using MicroserviceArchitecture.GameOfThrones.Infrastructure;
using MicroserviceArchitecture.GameOfThrones.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;

namespace MicroserviceArchitecture.GameOfThrones.Infra.CrossCutting.IoC.Modules
{
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

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
    .AsImplementedInterfaces();

            builder.Register<SingleInstanceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { return componentContext.TryResolve(t, out var o) ? o : null; };
            });

            builder.Register<MultiInstanceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();

                return t => (IEnumerable<object>)componentContext.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });

            builder.RegisterType<InMemoryBus>()
               .As<IEventBus>()
               .AsSelf()
               .InstancePerLifetimeScope();

            builder.RegisterType<EventStore>()
               .As<IEventStore>()
               .InstancePerLifetimeScope();

            builder.RegisterType<EventStoreRepository>()
              .As<IEventStoreRepository>()
              .InstancePerLifetimeScope();

            builder.RegisterType<EventStoreContext>()
               .As<IDbContext>()
               .AsSelf()
               .InstancePerLifetimeScope();
        }
    }
}