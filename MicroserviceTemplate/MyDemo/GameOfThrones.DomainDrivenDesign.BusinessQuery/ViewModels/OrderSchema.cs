using GraphQL.Types;
using System;

namespace MicroserviceArchitecture.GameOfThrones.BusinessQuery.ViewModels
{
    using Queries;

    public class OrderSchema : Schema
    {
        public OrderSchema(Func<Type, GraphType> resolveType)
            : base(resolveType)
        {
            Query = (OrderQuery)resolveType(typeof(OrderQuery));
        }
    }
}