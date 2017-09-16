using System;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus
{
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        public class SubscriptionInfo
        {
            public Type HandlerType { get; }
            public bool IsDynamic { get; }

            private SubscriptionInfo(bool isDynamic, Type handlerType)
            {
                IsDynamic = isDynamic;
                HandlerType = handlerType;
            }

            public static SubscriptionInfo Dynamic(Type handlerType)
            {
                return new SubscriptionInfo(true, handlerType);
            }

            public static SubscriptionInfo Typed(Type handlerType)
            {
                return new SubscriptionInfo(false, handlerType);
            }
        }
    }
}