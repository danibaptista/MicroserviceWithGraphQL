using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MicroserviceArchitecture.GameOfThrones.API.Controllers
{
    using DDD.EventSourcing.Core.Bus;
    using DDD.EventSourcing.Core.Commands;
    using Infrastructure.Services;
    using MicroserviceArchitecture.GameOfThrones.BusinessQuery.Queries;
    using MicroserviceArchitecture.GameOfThrones.Domain.WriteModel;

    [Route("api/v1/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly IEventBus _eventBus;
        private readonly IIdentityService _identityService;
        private readonly ISchema _schema;

        public OrdersController(IEventBus mediator, IDocumentExecuter documentExecuter, ISchema schema, IIdentityService identityService)
        {
            _eventBus = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _documentExecuter = documentExecuter ?? throw new ArgumentNullException(nameof(documentExecuter));
            _schema = schema ?? throw new ArgumentNullException(nameof(schema));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [Route("cancel")]
        [HttpPut]
        public async Task<IActionResult> CancelOrder([FromBody]CancelOrderCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var result = CommandResponse.Fail;
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var request = new IdentifiedCommand<CancelOrderCommand, CommandResponse>(command, guid);
                result = await _eventBus.SendCommand(request);
            }

            return result.Success ? (IActionResult)Ok() : (IActionResult)BadRequest();
        }

        [Route("create")]
        [HttpPut]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var result = CommandResponse.Fail;
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var request = new IdentifiedCommand<CreateOrderCommand, CommandResponse>(command, guid);
                result = await _eventBus.SendCommand(request);
            }

            return result.Success ? (IActionResult)Ok() : (IActionResult)BadRequest();
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