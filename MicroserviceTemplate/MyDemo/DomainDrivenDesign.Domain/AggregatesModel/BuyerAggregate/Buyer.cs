using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroserviceArchitecture.GameOfThrones.Domain.AggregatesModel.BuyerAggregate
{
    using DDD.Domain.Core.Models;
    using DDD.Domain.Core.SeedWork;
    using Events;

    public class Buyer
      : Entity, IAggregateRoot
    {
        private List<PaymentMethod> _paymentMethods;
        public Guid IdentityGuid { get; private set; }
        public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

        public Buyer(Guid identity) : this()
        {
            IdentityGuid = identity != default(Guid) ? identity : throw new ArgumentNullException(nameof(identity));
        }

        protected Buyer()
        {
            _paymentMethods = new List<PaymentMethod>();
        }

        public PaymentMethod VerifyOrAddPaymentMethod(
            int cardTypeId, string alias, string cardNumber,
            string securityNumber, string cardHolderName, DateTime expiration, Guid orderId)
        {
            var existingPayment = _paymentMethods.Where(p => p.IsEqualTo(cardTypeId, cardNumber, expiration))
                .SingleOrDefault();

            if (existingPayment != null)
            {
                AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));

                return existingPayment;
            }
            else
            {
                var payment = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

                _paymentMethods.Add(payment);

                AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId));

                return payment;
            }
        }
    }
}