using DDD.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace MicroserviceArchitecture.GameOfThrones.Domain.AggregatesModel.OrderAggregate
{
    public class Address
        : ValueObject
    {
        public String City { get; private set; }
        public String Country { get; private set; }
        public String State { get; private set; }
        public String Street { get; private set; }
        public String ZipCode { get; private set; }

        public Address(string street, string city, string state, string country, string zipcode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
        }

        private Address()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
            yield return ZipCode;
        }
    }
}