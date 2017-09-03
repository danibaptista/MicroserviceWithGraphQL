using Autofac;
using GraphQL.Types;

namespace MicroserviceArchitecture.GameOfThrones.API.Infrastructure.AutofacModules
{
    using BusinessQuery.Queries;
    using BusinessQuery.ViewModels;
    using GraphQL;

    public class BusinessQueryModule
        : Module
    {
        public BusinessQueryModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
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