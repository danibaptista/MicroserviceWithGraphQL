using Autofac;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Reflection;

namespace MicroserviceArchitecture.GameOfThrones.API.Infrastructure.AutofacModules
{
    using Autofac.Core;
    using BusinessCommand.Commands;
    using BusinessCommand.Decorators;
    using BusinessCommand.Validations;

    public class BusinessCommandModule
        : Autofac.Module
    {
        public BusinessCommandModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Register all the Command classes (they implement IAsyncRequestHandler) in assembly
            // holding the Commands
            builder.RegisterAssemblyTypes(typeof(CreateOrderCommand).GetTypeInfo().Assembly)
                .As(o => o.GetInterfaces()
                    .Where(i => i.IsClosedTypeOf(typeof(IAsyncRequestHandler<,>)))
                    .Select(i => new KeyedService(typeof(IAsyncRequestHandler<,>).Name, i)));

            builder
               .RegisterAssemblyTypes(typeof(CreateOrderCommandValidator).GetTypeInfo().Assembly)
               .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
               .AsImplementedInterfaces();

            builder.RegisterGenericDecorator(typeof(LogDecorator<,>),
          typeof(IAsyncRequestHandler<,>),
          typeof(IAsyncRequestHandler<,>).Name)
          .Keyed("commandHandlerDecorator", typeof(IAsyncRequestHandler<,>));

            builder.RegisterGenericDecorator(typeof(ValidatorDecorator<,>),
                    typeof(IAsyncRequestHandler<,>),
                    fromKey: "commandHandlerDecorator");
        }
    }
}