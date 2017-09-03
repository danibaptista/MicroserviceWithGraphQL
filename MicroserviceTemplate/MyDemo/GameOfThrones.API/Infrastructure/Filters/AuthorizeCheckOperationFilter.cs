using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MicroserviceArchitecture.GameOfThrones.API.Infrastructure.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // Check for authorize attribute
            var hasAuthorize = context.ApiDescription.ControllerAttributes().OfType<AuthorizeAttribute>().Any() ||
                               context.ApiDescription.ActionAttributes().OfType<AuthorizeAttribute>().Any();

            if (hasAuthorize)
            {
                operation.Responses.Add(((int)HttpStatusCode.Unauthorized).ToString(), new Response { Description = Enum.GetName(typeof(HttpStatusCode), HttpStatusCode.Unauthorized) });
                operation.Responses.Add(((int)HttpStatusCode.Forbidden).ToString(), new Response { Description = Enum.GetName(typeof(HttpStatusCode), HttpStatusCode.Forbidden) });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
                operation.Security.Add(new Dictionary<string, IEnumerable<string>>
                {
                    { "oauth2", new [] { "orderingapi" } }
                });
            }
        }
    }
}