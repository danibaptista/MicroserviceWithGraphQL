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
                  var orderId = context.GetArgument<Guid>("id");
                  var order = orderRepository.GetAsync(orderId).Result;
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