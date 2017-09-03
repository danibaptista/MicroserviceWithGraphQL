using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroserviceArchitecture.GameOfThrones.Domain.AggregatesModel.OrderAggregate
{
    using DDD.Domain.Core.Exceptions;
    using DDD.Domain.Core.Models;
    using DDD.Domain.Core.SeedWork;
    using Events;

    public class Order
        : Entity, IAggregateRoot
    {
        // DDD Patterns comment Using a private collection field, better for DDD Aggregate's
        // encapsulation so OrderItems cannot be added from "outside the AggregateRoot" directly to
        // the collection, but only through the method OrderAggrergateRoot.AddOrderItem() which
        // includes behaviour.
        private readonly List<OrderItem> orderItems;

        private Guid? buyerId;

        private string description;

        // DDD Patterns comment Using private fields, allowed since EF Core 1.1, is a much better
        // encapsulation aligned with DDD Aggregates and Domain Entities (Instead of properties and
        // property collections)
        private DateTime orderDate;

        private int orderStatusId;
        private Guid? paymentMethodId;
        public Address Address { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => orderItems.AsReadOnly();
        public OrderStatus OrderStatus { get; private set; }

        public Order(Guid userId, Address address, int cardTypeId, string cardNumber, string cardSecurityNumber,
                        string cardHolderName, DateTime cardExpiration, Guid? buyerId = null, Guid? paymentMethodId = null)
        {
            orderItems = new List<OrderItem>();
            this.buyerId = buyerId;
            this.paymentMethodId = paymentMethodId;
            orderStatusId = OrderStatus.Submitted.Id;
            orderDate = DateTime.UtcNow;
            Address = address;

            // Add the OrderStarterDomainEvent to the domain events collection to be
            // raised/dispatched when comitting changes into the Database [ After
            // DbContext.SaveChanges() ]
            AddOrderStartedDomainEvent(userId, cardTypeId, cardNumber,
                                       cardSecurityNumber, cardHolderName, cardExpiration);
        }

        // Using List<>.AsReadOnly()
        // This will create a read only wrapper around the private list so is protected against "external updates".
        // It's much cheaper than .ToList() because it will not have to copy all items in a new collection. (Just one heap alloc for the wrapper instance)
        //https://msdn.microsoft.com/en-us/library/e78dcd75(v=vs.110).aspx
        protected Order()
        {
            orderItems = new List<OrderItem>();
        }

        // DDD Patterns comment This Order AggregateRoot's method "AddOrderitem()" should be the only
        // way to add Items to the Order, so any behavior (discounts, etc.) and validations are
        // controlled by the AggregateRoot in order to maintain consistency between the whole Aggregate.
        public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
        {
            var existingOrderForProduct = orderItems.Where(o => o.ProductId == productId)
                .SingleOrDefault();

            if (existingOrderForProduct != null)
            {
                //if previous line exist modify it with higher discount  and units..

                if (discount > existingOrderForProduct.GetCurrentDiscount())
                {
                    existingOrderForProduct.SetNewDiscount(discount);
                    existingOrderForProduct.AddUnits(units);
                }
            }
            else
            {
                //add validated new order item

                var orderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);
                orderItems.Add(orderItem);
            }
        }

        public void SetBuyerId(Guid id)
        {
            buyerId = id;
        }

        public void SetCancelledStatus()
        {
            if (orderStatusId == OrderStatus.Paid.Id ||
                orderStatusId == OrderStatus.Shipped.Id)
            {
                StatusChangeException(OrderStatus.Cancelled);
            }

            orderStatusId = OrderStatus.Cancelled.Id;
            description = $"The order was cancelled.";
        }

        public void SetCancelledStatusWhenStockIsRejected(IEnumerable<int> orderStockRejectedItems)
        {
            if (orderStatusId != OrderStatus.AwaitingValidation.Id)
            {
                StatusChangeException(OrderStatus.Cancelled);
            }

            orderStatusId = OrderStatus.Cancelled.Id;

            var itemsStockRejectedProductNames = OrderItems
                .Where(c => orderStockRejectedItems.Contains(c.ProductId))
                .Select(c => c.GetOrderItemProductName());

            var itemsStockRejectedDescription = string.Join(", ", itemsStockRejectedProductNames);
            description = $"The product items don't have stock: ({itemsStockRejectedDescription}).";
        }

        public void SetPaidStatus()
        {
            if (orderStatusId != OrderStatus.StockConfirmed.Id)
            {
                StatusChangeException(OrderStatus.Paid);
            }

            AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(Id, OrderItems));

            orderStatusId = OrderStatus.Paid.Id;
            description = "The payment was performed at a simulated \"American Bank checking bank account endinf on XX35071\"";
        }

        public void SetPaymentId(Guid id)
        {
            paymentMethodId = id;
        }

        public void SetShippedStatus()
        {
            if (orderStatusId != OrderStatus.Paid.Id)
            {
                StatusChangeException(OrderStatus.Shipped);
            }

            orderStatusId = OrderStatus.Shipped.Id;
            description = "The order was shipped.";
        }

        private void AddOrderStartedDomainEvent(Guid userId, int cardTypeId, string cardNumber,
                string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)
        {
            var orderStartedDomainEvent = new OrderStartedDomainEvent(
                this, userId, cardTypeId, cardNumber, cardSecurityNumber,
                cardHolderName, cardExpiration);

            AddDomainEvent(orderStartedDomainEvent);
        }

        private void StatusChangeException(OrderStatus orderStatusToChange)
        {
            throw new DomainException($"Not possible to change order status from {OrderStatus.Name} to {orderStatusToChange.Name}.");
        }
    }
}