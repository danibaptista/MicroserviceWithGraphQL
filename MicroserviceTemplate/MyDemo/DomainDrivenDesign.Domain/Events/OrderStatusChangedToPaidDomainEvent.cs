using System.Collections.Generic;

namespace MicroserviceArchitecture.GameOfThrones.Domain.Events
{
    using AggregatesModel.OrderAggregate;
    using DDD.EventSourcing.Core.Events;
    using System;

    /// <summary>
    /// Event used when the order is paid
    /// </summary>
    public class OrderStatusChangedToPaidDomainEvent
        : Event
    {
        public Guid OrderId { get; }
        public IEnumerable<OrderItem> OrderItems { get; }

        public OrderStatusChangedToPaidDomainEvent(Guid orderId,
            IEnumerable<OrderItem> orderItems)
        {
            OrderId = orderId;
            OrderItems = orderItems;
            AggregateId = orderId;
        }
    }
}