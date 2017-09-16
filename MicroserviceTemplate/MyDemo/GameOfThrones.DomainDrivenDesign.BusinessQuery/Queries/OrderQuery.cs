using GraphQL.Types;

namespace MicroserviceArchitecture.GameOfThrones.BusinessQuery.Queries
{
    using Domain.AggregatesModel.BuyerAggregate;
    using Domain.AggregatesModel.OrderAggregate;
    using System;
    using ViewModels;

    public class OrderQuery : ObjectGraphType
    {
        public OrderQuery()
        {
        }

        public OrderQuery(IOrderRepository orderRepository, IBuyerRepository buyerRepository)
        {
            Name = "Query";

            Field<OrderType>(
              "order",
              arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the order" }),
              resolve: context =>
              {
                  var orderId = Guid.TryParse(context.GetArgument<string>("id"), out var result) ? result : default(Guid);
                  var order = orderRepository.GetAsync(orderId, "OrderStatus").Result;
                  return order;
              }
            );

            Field<ListGraphType<OrderType>>(
             "orders",

             resolve: context =>
             {
                 return orderRepository.GetAllAsync().Result;
             }
           );
        }
    }
}