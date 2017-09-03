using DDD.Domain.Core.Exceptions;
using DDD.Domain.Core.Models;
using System;

namespace MicroserviceArchitecture.GameOfThrones.Domain.AggregatesModel.OrderAggregate
{
    public class OrderItem
        : Entity
    {
        private decimal _discount;

        private string _pictureUrl;

        // DDD Patterns comment Using private fields, allowed since EF Core 1.1, is a much better
        // encapsulation aligned with DDD Aggregates and Domain Entities (Instead of properties and
        // property collections)
        private string _productName;

        private decimal _unitPrice;
        private int _units;

        public int ProductId { get; private set; }

        public OrderItem(int productId, string productName, decimal unitPrice, decimal discount, string PictureUrl, int units = 1)
        {
            if (units <= 0)
            {
                throw new DomainException("Invalid number of units");
            }

            if ((unitPrice * units) < discount)
            {
                throw new DomainException("The total of order item is lower than applied discount");
            }

            ProductId = productId;

            _productName = productName;
            _unitPrice = unitPrice;
            _discount = discount;
            _units = units;
            _pictureUrl = PictureUrl;
        }

        protected OrderItem()
        {
        }

        public void AddUnits(int units)
        {
            if (units < 0)
            {
                throw new DomainException("Invalid units");
            }

            _units += units;
        }

        public decimal GetCurrentDiscount()
        {
            return _discount;
        }

        public string GetOrderItemProductName() => _productName;

        public int GetUnits()
        {
            return _units;
        }

        public void SetNewDiscount(decimal discount)
        {
            if (discount < 0)
            {
                throw new DomainException("Discount is not valid");
            }

            _discount = discount;
        }

        public void SetPictureUri(string pictureUri)
        {
            if (!String.IsNullOrWhiteSpace(pictureUri))
            {
                _pictureUrl = pictureUri;
            }
        }
    }
}