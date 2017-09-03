using DDD.EventSourcing.Core.Commands;
using System;
using System.Runtime.Serialization;

namespace MicroserviceArchitecture.GameOfThrones.BusinessCommand.Commands
{
    public class CancelOrderCommand : Command<CommandResponse>
    {
        [DataMember]
        public Guid OrderNumber { get; private set; }

        public CancelOrderCommand(Guid orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }
}