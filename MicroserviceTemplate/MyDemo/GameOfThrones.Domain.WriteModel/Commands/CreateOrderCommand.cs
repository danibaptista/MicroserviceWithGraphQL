﻿using DDD.EventSourcing.Core.Commands;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MicroserviceArchitecture.GameOfThrones.Domain.WriteModel
{
    // DDD and CQRS patterns comment: Note that it is recommended to implement immutable Commands In
    // this case, its immutability is achieved by having all the setters as private plus only being
    // able to update the data just once, when creating the object through its constructor.
    // References on Immutable Commands: http://cqrs.nu/Faq
    // https://docs.spine3.org/motivation/immutability.html
    // http://blog.gauffin.org/2012/06/griffin-container-introducing-command-support/ https://msdn.microsoft.com/en-us/library/bb383979.aspx

    [DataContract]
    public class CreateOrderCommand
        : Command<CommandResponse>
    {
        [DataMember]
        private readonly List<OrderItemDTO> orderItemsDto;

        [DataMember]
        public DateTime CardExpiration { get; private set; }

        [DataMember]
        public string CardHolderName { get; private set; }

        [DataMember]
        public string CardNumber { get; private set; }

        [DataMember]
        public string CardSecurityNumber { get; private set; }

        [DataMember]
        public int CardTypeId { get; private set; }

        [DataMember]
        public string City { get; private set; }

        [DataMember]
        public string Country { get; private set; }

        [DataMember]
        public IReadOnlyCollection<OrderItemDTO> OrderItems => orderItemsDto.AsReadOnly();

        [DataMember]
        public string State { get; private set; }

        [DataMember]
        public string Street { get; private set; }

        [DataMember]
        public Guid UserId { get; private set; }

        [DataMember]
        public string ZipCode { get; private set; }

        public CreateOrderCommand()
        {
            orderItemsDto = new List<OrderItemDTO>();
        }

        public CreateOrderCommand(List<OrderItemDTO> orderItemsDto, Guid userId, string city, string street, string state, string country, string zipcode,
            string cardNumber, string cardHolderName, DateTime cardExpiration,
            string cardSecurityNumber, int cardTypeId) : this()
        {
            this.orderItemsDto = orderItemsDto;
            UserId = userId;
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipcode;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            CardSecurityNumber = cardSecurityNumber;
            CardTypeId = cardTypeId;
            CardExpiration = cardExpiration;
        }
    }
}