using DDD.EventSourcing.Core.Bus;
using DDD.EventSourcing.Core.Commands;
using DDD.EventSourcing.Core.Events;
using DDD.Infrastructure.Idempotency;
using MicroserviceArchitecture.GameOfThrones.Domain.WriteModel;
using System;
using System.Threading.Tasks;

namespace MicroserviceArchitecture.GameOfThrones.Domain.WriteService.CommandHandlers
{
    /// <summary>
    /// Provides a base implementation for handling duplicate request and ensuring idempotent
    /// updates, in the cases where a requestid sent by client is used to detect duplicate requests.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the command handler that performs the operation if request is not duplicated
    /// </typeparam>
    /// <typeparam name="R">Return value of the inner command handler</typeparam>
    public class IdentifierCommandHandler<T, R> : ICommandHandler<IdentifiedCommand<T, R>, R>
        where T : Command<R>
        where R : CommandResponse
    {
        private readonly IEventBus mediator;
        private readonly IRequestManager requestManager;

        public IdentifierCommandHandler(IEventBus mediator, IRequestManager requestManager)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager));
        }

        /// <summary> This method handles the command. It just ensures that no other request exists
        /// with the same ID, and if this is the case just enqueues the original inner command.
        /// </summary> <param name="message">IdentifiedCommand which contains both original command &
        /// request ID</param> <returns>Return value of inner command or default value if request
        /// same ID was found</returns>
        public async Task<R> Handle(IdentifiedCommand<T, R> message)
        {
            var alreadyExists = await requestManager.ExistAsync(message.Id);
            if (alreadyExists)
            {
                return CreateResultForDuplicateRequest();
            }
            else
            {
                await requestManager.CreateRequestForCommandAsync<T>(message.Id);

                var result = await mediator.SendCommand(message.Command);

                return result;
            }
        }

        /// <summary>
        /// Creates the result value to return if a previous request was found
        /// </summary>
        /// <returns></returns>
        protected virtual R CreateResultForDuplicateRequest()
        {
            return default(R);
        }
    }
}