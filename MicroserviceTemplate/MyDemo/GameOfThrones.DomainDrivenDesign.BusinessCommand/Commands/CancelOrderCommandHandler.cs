using System.Threading.Tasks;

namespace MicroserviceArchitecture.GameOfThrones.BusinessCommand.Commands
{
    using DDD.EventSourcing.Core.Bus;
    using DDD.EventSourcing.Core.Commands;
    using DDD.EventSourcing.Core.Events;
    using DDD.Infrastructure.Idempotency;
    using Domain.AggregatesModel.OrderAggregate;

    public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand, CommandResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public CancelOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Handler which processes the command when customer executes cancel order from app
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<CommandResponse> Handle(CancelOrderCommand command)
        {
            var orderToUpdate = await _orderRepository.GetAsync(command.OrderNumber);
            orderToUpdate.SetCancelledStatus();
            return await _orderRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }

    public class CancelOrderCommandIdentifiedHandler : IdentifierCommandHandler<CancelOrderCommand, CommandResponse>
    {
        public CancelOrderCommandIdentifiedHandler(IEventBus mediator, IRequestManager requestManager) : base(mediator, requestManager)
        {
        }

        protected override CommandResponse CreateResultForDuplicateRequest()
        {
            return CommandResponse.Ok;                // Ignore duplicate requests for processing order.
        }
    }
}