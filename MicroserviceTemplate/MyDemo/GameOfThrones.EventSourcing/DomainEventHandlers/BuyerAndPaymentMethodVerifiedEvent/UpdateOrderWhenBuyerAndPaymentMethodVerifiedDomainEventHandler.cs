using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MicroserviceArchitecture.GameOfThrones.EventSourcing.DomainEventHandlers.BuyerAndPaymentMethodVerified
{
    using DDD.EventSourcing.Core.Events;
    using Domain.AggregatesModel.OrderAggregate;
    using Domain.Events;

    public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler
                   : IEventHandler<BuyerAndPaymentMethodVerifiedDomainEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IOrderRepository orderRepository;

        public UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(
            IOrderRepository orderRepository, ILoggerFactory logger)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Domain Logic comment: When the Buyer and Buyer's payment method have been created or
        // verified that they existed, then we can update the original Order with the BuyerId and
        // PaymentId (foreign keys)
        public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent buyerPaymentMethodVerifiedEvent)
        {
            var orderToUpdate = await orderRepository.GetAsync(buyerPaymentMethodVerifiedEvent.OrderId);
            orderToUpdate.SetBuyerId(buyerPaymentMethodVerifiedEvent.Buyer.Id);
            orderToUpdate.SetPaymentId(buyerPaymentMethodVerifiedEvent.Payment.Id);

            logger.CreateLogger(nameof(UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler))
                .LogTrace($"Order with Id: {buyerPaymentMethodVerifiedEvent.OrderId} has been successfully updated with a payment method id: { buyerPaymentMethodVerifiedEvent.Payment.Id }");
        }
    }
}