namespace MicroserviceArchitecture.GameOfThrones.Domain.Events
{
    using AggregatesModel.BuyerAggregate;
    using DDD.EventSourcing.Core.Events;
    using System;

    public class BuyerAndPaymentMethodVerifiedDomainEvent
        : Event
    {
        public Buyer Buyer { get; private set; }
        public Guid OrderId { get; private set; }
        public PaymentMethod Payment { get; private set; }

        public BuyerAndPaymentMethodVerifiedDomainEvent(Buyer buyer, PaymentMethod payment, Guid orderId)
        {
            Buyer = buyer;
            Payment = payment;
            OrderId = orderId;
            AggregateId = orderId;
        }
    }
}