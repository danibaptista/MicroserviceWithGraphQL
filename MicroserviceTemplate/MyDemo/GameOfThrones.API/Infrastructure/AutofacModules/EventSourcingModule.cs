using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusRabbitMQ;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusServiceBus;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF;
using Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Data.Common;

namespace MicroserviceArchitecture.GameOfThrones.API.Infrastructure.AutofacModules
{
    public class EventSourcingModule
        : Autofac.Module
    {
        private readonly IConfigurationRoot configuration;
        private readonly IServiceCollection services;

        public EventSourcingModule(IServiceCollection services, IConfigurationRoot configuration)
        {
            this.services = services;
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var integrationEventLogContext = new IntegrationEventLogContext(
               new DbContextOptionsBuilder<IntegrationEventLogContext>()
               .UseSqlServer(configuration["ConnectionString"], b => b.MigrationsAssembly("MicroserviceArchitecture.GameOfThrones.API"))
               .Options);
            integrationEventLogContext.Database.Migrate();

            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
               sp => (DbConnection c) => new IntegrationEventLogService(c));

            //services.AddTransient<IOrderingIntegrationEventService, OrderingIntegrationEventService>();

            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton<IServiceBusPersisterConnection>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                    var serviceBusConnectionString = configuration["EventBusConnection"];
                    var serviceBusConnection = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);

                    return new DefaultServiceBusPersisterConnection(serviceBusConnection, logger);
                });
            }
            else
            {
                services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                    var factory = new ConnectionFactory()
                    {
                        HostName = configuration["EventBusConnection"]
                    };

                    return new DefaultRabbitMQPersistentConnection(factory, logger);
                });
            }
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            //eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>>();
            //eventBus.Subscribe<OrderPaymentSuccededIntegrationEvent, IIntegrationEventHandler<OrderPaymentSuccededIntegrationEvent>>();
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
                {
                    var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                    var subscriptionClientName = configuration["SubscriptionClientName"];

                    return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                        eventBusSubcriptionsManager, subscriptionClientName, iLifetimeScope);
                });
            }
            else
            {
                services.AddSingleton<IEventBus, EventBusRabbitMQ>();
            }

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}