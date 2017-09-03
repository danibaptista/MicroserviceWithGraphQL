using GraphQL;
using GraphQL.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MicroserviceArchitecture.GameOfThrones.API.Controllers
{
    using BusinessCommand.Commands;
    using BusinessQuery.Queries;
    using Infrastructure.Services;

    [Route("api/v1/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;
        private readonly ISchema _schema;

        public OrdersController(IMediator mediator, IDocumentExecuter documentExecuter, ISchema schema, IIdentityService identityService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _documentExecuter = documentExecuter ?? throw new ArgumentNullException(nameof(documentExecuter));
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [Route("cancel")]
        [HttpPut]
        public async Task<IActionResult> CancelOrder([FromBody]CancelOrderCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var requestCancelOrder = new IdentifiedCommand<CancelOrderCommand, bool>(command, guid);
                commandResult = await _mediator.Send(requestCancelOrder);
            }

            return commandResult ? (IActionResult)Ok() : (IActionResult)BadRequest();
        }

        [Route("graphql")]
        [HttpPut]
        public async Task<IActionResult> Get([FromBody] GraphQLQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            var executionOptions = new ExecutionOptions { Schema = _schema, Query = query.Query };

            try
            {
                var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

                if (result.Errors?.Count > 0)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}