using DDD.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroserviceArchitecture.GameOfThrones.Domain.AggregatesModel.BuyerAggregate
{
    public class CardType
        : Enumeration
    {
        public static CardType Amex = new CardType(1, "Amex");
        public static CardType MasterCard = new CardType(3, "MasterCard");
        public static CardType Visa = new CardType(2, "Visa");

        public CardType(int id, string name)
            : base(id, name)
        {
        }

        protected CardType()
        {
        }

        public static CardType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ArgumentException($"Possible values for CardType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static CardType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new ArgumentException($"Possible values for CardType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static IEnumerable<CardType> List()
        {
            return new[] { Amex, Visa, MasterCard };
        }
    }
}