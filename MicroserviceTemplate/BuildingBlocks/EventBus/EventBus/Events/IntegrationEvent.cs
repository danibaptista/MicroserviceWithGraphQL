using System;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events
{
    public class IntegrationEvent
    {
        public DateTime CreationDate { get; }

        public Guid Id { get; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
    }
}