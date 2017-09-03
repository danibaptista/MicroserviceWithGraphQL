using DDD.Domain.Core.Exceptions;
using DDD.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroserviceArchitecture.GameOfThrones.Domain.AggregatesModel.OrderAggregate
{
    public class OrderStatus
        : Enumeration
    {
        public static OrderStatus AwaitingValidation = new OrderStatus(2, nameof(AwaitingValidation).ToLowerInvariant());
        public static OrderStatus Cancelled = new OrderStatus(6, nameof(Cancelled).ToLowerInvariant());
        public static OrderStatus Paid = new OrderStatus(4, nameof(Paid).ToLowerInvariant());
        public static OrderStatus Shipped = new OrderStatus(5, nameof(Shipped).ToLowerInvariant());
        public static OrderStatus StockConfirmed = new OrderStatus(3, nameof(StockConfirmed).ToLowerInvariant());
        public static OrderStatus Submitted = new OrderStatus(1, nameof(Submitted).ToLowerInvariant());

        public OrderStatus(int id, string name)
            : base(id, name)
        {
        }

        protected OrderStatus()
        {
        }

        public static OrderStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new DomainException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static OrderStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new DomainException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static IEnumerable<OrderStatus> List() =>
                    new[] { Submitted, AwaitingValidation, StockConfirmed, Paid, Shipped, Cancelled };
    }
}