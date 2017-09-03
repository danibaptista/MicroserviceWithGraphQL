using GraphQL.Types;

namespace MicroserviceArchitecture.GameOfThrones.BusinessQuery.ViewModels
{
    using Domain.AggregatesModel.OrderAggregate;

    public class OrderType : ObjectGraphType<Order>
    {
        public OrderType()
        {
            Name = "Order";

            Field(d => d.Id).Description("The id of the order.");
            // Field(d => d.Address, nullable: true).Description("The name of the character.");
            Field(d => d.OrderStatus.Name).Description("The order status.").Name("status");

            // Field<ListGraphType<OrderItemType>>("items");
        }
    }
}