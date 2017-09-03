using Autofac;
using Autofac.Core;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MicroserviceArchitecture.GameOfThrones.API.Infrastructure.AutofacModules
{
    using EventSourcing.DomainEventHandlers.OrderStartedEvent;

    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            // Register all the event classes (they implement IAsyncNotificationHandler) in assembly
            // holding the Commands
            builder.RegisterAssemblyTypes(typeof(ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler).GetTypeInfo().Assembly)
                .As(o => o.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof(IAsyncNotificationHandler<>)))
                    .Select(i => new KeyedService(typeof(IAsyncNotificationHandler<>).Name, i)))
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
        }
    }
}