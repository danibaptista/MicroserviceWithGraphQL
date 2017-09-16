using System;
using System.Threading.Tasks;

namespace MicroserviceArchitecture.GameOfThrones.Domain.WriteService.CommandHandlers
{
    using DDD.EventSourcing.Core.Bus;
    using DDD.EventSourcing.Core.Commands;
    using DDD.EventSourcing.Core.Events;
    using DDD.Infrastructure.Idempotency;
    using Domain.AggregatesModel.OrderAggregate;
    using MicroserviceArchitecture.GameOfThrones.Domain.WriteModel;

    public class CreateOrderCommandHandler
        : ICommandHandler<CreateOrderCommand, CommandResponse>
    {
        private readonly IEventBus mediator;
        private readonly IOrderRepository orderRepository;

        // Using DI to inject infrastructure persistence Repositories
        public CreateOrderCommandHandler(IEventBus mediator, IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<CommandResponse> Handle(CreateOrderCommand message)
        {
            // Add/Update the Buyer AggregateRoot DDD patterns comment: Add child entities and
            // value-objects through the Order Aggregate-Root methods and constructor so validations,
            // invariants and business logic make sure that consistency is preserved across the whole aggregate
            var address = new Address(message.Street, message.City, message.State, message.Country, message.ZipCode);
            var order = new Order(message.UserId, address, message.CardTypeId, message.CardNumber, message.CardSecurityNumber, message.CardHolderName, message.CardExpiration);

            foreach (var item in message.OrderItems)
            {
                order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
            }

            orderRepository.Add(order);

            return await orderRepository.UnitOfWork
                .SaveEntitiesAsync();
        }
    }

    public class CreateOrderCommandIdentifiedHandler : IdentifierCommandHandler<CreateOrderCommand, CommandResponse>
    {
        public CreateOrderCommandIdentifiedHandler(IEventBus mediator, IRequestManager requestManager) : base(mediator, requestManager)
        {
        }

        protected override CommandResponse CreateResultForDuplicateRequest()
        {
            return CommandResponse.Ok;                // Ignore duplicate requests for creating order.
        }
    }
}