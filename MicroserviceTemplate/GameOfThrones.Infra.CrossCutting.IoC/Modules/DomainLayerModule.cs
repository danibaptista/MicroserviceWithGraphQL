using Autofac;
using DDD.EventSourcing.Core.Bus;
using DDD.EventSourcing.Core.Events;
using DDD.Infra.CrossCutting.Bus.Core;
using DDD.Infra.EventSourcing.Core.Contexts;
using DDD.Infra.EventSourcing.Core.EventSourcing;
using DDD.Infra.EventSourcing.Core.Repositories;
using DDD.Infrastructure;
using DDD.Infrastructure.Idempotency;
using FluentValidation;
using GraphQL;
using GraphQL.Types;
using MediatR;
using MicroserviceArchitecture.GameOfThrones.BusinessQuery.Queries;
using MicroserviceArchitecture.GameOfThrones.BusinessQuery.ViewModels;
using MicroserviceArchitecture.GameOfThrones.Domain.AggregatesModel.BuyerAggregate;
using MicroserviceArchitecture.GameOfThrones.Domain.AggregatesModel.OrderAggregate;
using MicroserviceArchitecture.GameOfThrones.Domain.WriteModel;
using MicroserviceArchitecture.GameOfThrones.Domain.WriteService.CommandHandlers;
using MicroserviceArchitecture.GameOfThrones.Domain.WriteService.Decorators;
using MicroserviceArchitecture.GameOfThrones.Domain.WriteService.Validations;
using MicroserviceArchitecture.GameOfThrones.EventSourcing.DomainEventHandlers.OrderStartedEvent;
using MicroserviceArchitecture.GameOfThrones.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MicroserviceArchitecture.GameOfThrones.Infra.CrossCutting.IoC.Modules
{
    public class DomainLayerModule
        : Autofac.Module
    {
        public DomainLayerModule()
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

            builder.RegisterType<EventStoreContext>()
              .AsSelf();

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

            // Register all the Command classes (they implement IAsyncRequestHandler) in assembly
            // holding the Commands
            builder.RegisterAssemblyTypes(typeof(CreateOrderCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<,>));

            // Register all the Command classes (they implement IAsyncRequestHandler) in assembly
            // holding the Commands
            builder.RegisterAssemblyTypes(typeof(CreateOrderCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IAsyncRequestHandler<,>));

            // Register all the event classes (they implement IAsyncNotificationHandler) in assembly
            // holding the Commands
            builder.RegisterAssemblyTypes(typeof(ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IAsyncNotificationHandler<>));

            builder
                .RegisterAssemblyTypes(typeof(CreateOrderCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.RegisterType<OrderQuery>()
             .AsSelf()
             .InstancePerLifetimeScope();

            builder.RegisterType<DocumentExecuter>()
               .As<IDocumentExecuter>()
               .InstancePerLifetimeScope();

            builder.RegisterType<OrderType>()
               .AsSelf()
               .InstancePerDependency();

            builder.Register<ISchema>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return new OrderSchema(type => (GraphType)context.Resolve(type)) { Query = context.Resolve<OrderQuery>() };
            })
            .InstancePerLifetimeScope();
        }
    }
}