//namespace MicroserviceArchitecture.GameOfThrones.EventSourcing.IntegrationEvents.EventHandling
//{
//    using Domain.AggregatesModel.OrderAggregate;
//    using Events;
//    using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
//    using System.Threading.Tasks;

// public class OrderPaymentSuccededIntegrationEventHandler :
// IIntegrationEventHandler<OrderPaymentSuccededIntegrationEvent> { private readonly IOrderRepository _orderRepository;

// public OrderPaymentSuccededIntegrationEventHandler(IOrderRepository orderRepository) {
// _orderRepository = orderRepository; }

// public async Task Handle(OrderPaymentSuccededIntegrationEvent @event) { var orderToUpdate = await _orderRepository.Get(@event.OrderId);

// orderToUpdate.SetPaidStatus();

//            await _orderRepository.UnitOfWork.SaveEntitiesAsync();
//        }
//    }
//}