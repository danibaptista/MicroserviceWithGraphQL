using DDD.Domain.Core.Exceptions;
using DDD.Domain.Core.Models;
using System;

namespace MicroserviceArchitecture.GameOfThrones.Domain.AggregatesModel.BuyerAggregate
{
    public class PaymentMethod
        : Entity
    {
        private string _alias;
        private string _cardHolderName;
        private string _cardNumber;
        private int _cardTypeId;
        private DateTime _expiration;
        private string _securityNumber;
        public CardType CardType { get; private set; }

        public PaymentMethod(int cardTypeId, string alias, string cardNumber, string securityNumber, string cardHolderName, DateTime expiration)
        {
            _cardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : throw new DomainException(nameof(cardNumber));
            _securityNumber = !string.IsNullOrWhiteSpace(securityNumber) ? securityNumber : throw new DomainException(nameof(securityNumber));
            _cardHolderName = !string.IsNullOrWhiteSpace(cardHolderName) ? cardHolderName : throw new DomainException(nameof(cardHolderName));

            if (expiration < DateTime.UtcNow)
            {
                throw new DomainException(nameof(expiration));
            }

            _alias = alias;
            _expiration = expiration;
            _cardTypeId = cardTypeId;
        }

        protected PaymentMethod()
        {
        }

        public bool IsEqualTo(int cardTypeId, string cardNumber, DateTime expiration)
        {
            return _cardTypeId == cardTypeId
                && _cardNumber == cardNumber
                && _expiration == expiration;
        }
    }
}