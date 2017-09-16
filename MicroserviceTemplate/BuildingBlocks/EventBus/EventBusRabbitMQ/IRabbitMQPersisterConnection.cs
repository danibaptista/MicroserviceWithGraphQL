using RabbitMQ.Client;
using System;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBusRabbitMQ
{
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        IModel CreateModel();

        bool TryConnect();
    }
}