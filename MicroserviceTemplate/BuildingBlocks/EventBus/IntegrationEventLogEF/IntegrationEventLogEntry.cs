using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using Newtonsoft.Json;
using System;

namespace Microsoft.eShopOnContainers.BuildingBlocks.IntegrationEventLogEF
{
    public class IntegrationEventLogEntry
    {
        public string Content { get; private set; }

        public DateTime CreationTime { get; private set; }

        public Guid EventId { get; private set; }

        public string EventTypeName { get; private set; }

        public EventStateEnum State { get; set; }

        public int TimesSent { get; set; }

        public IntegrationEventLogEntry(IntegrationEvent @event)
        {
            EventId = @event.Id;
            CreationTime = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = JsonConvert.SerializeObject(@event);
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
        }

        private IntegrationEventLogEntry()
        {
        }
    }
}