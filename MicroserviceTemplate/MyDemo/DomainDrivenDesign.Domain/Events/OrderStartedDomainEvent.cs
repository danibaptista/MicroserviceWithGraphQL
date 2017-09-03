using System;

namespace MicroserviceArchitecture.GameOfThrones.Domain.Events
{
    using AggregatesModel.OrderAggregate;
    using DDD.EventSourcing.Core.Events;

    /// <summary>
    /// Event used when an order is created
    /// </summary>
    public class OrderStartedDomainEvent
        : Event
    {
        public DateTime CardExpiration { get; private set; }
        public string CardHolderName { get; private set; }
        public string CardNumber { get; private set; }
        public string CardSecurityNumber { get; private set; }
        public int CardTypeId { get; private set; }
        public Order Order { get; private set; }
        public Guid UserId { get; private set; }

        public OrderStartedDomainEvent(Order order, Guid userId,
            int cardTypeId, string cardNumber,
            string cardSecurityNumber, string cardHolderName,
            DateTime cardExpiration)
        {
            Order = order;
            UserId = userId;
            CardTypeId = cardTypeId;
            CardNumber = cardNumber;
            CardSecurityNumber = cardSecurityNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            AggregateId = userId;
        }
    }
}