using DDD.EventSourcing.Core.Commands;
using DDD.EventSourcing.Core.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MicroserviceArchitecture.GameOfThrones.BusinessCommand.Decorators
{
    public class LogDecorator<TRequest, TResponse>
        : ICommandHandler<TRequest, TResponse>
        where TRequest : Command<TResponse>
        where TResponse : CommandResponse
    {
        private readonly ICommandHandler<TRequest, TResponse> inner;
        private readonly ILogger<LogDecorator<TRequest, TResponse>> logger;

        public LogDecorator(
            ICommandHandler<TRequest, TResponse> inner,
            ILogger<LogDecorator<TRequest, TResponse>> logger)
        {
            this.inner = inner;
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest message)
        {
            logger.LogInformation($"Executing command {inner.GetType().FullName}");

            var response = await inner.Handle(message);

            logger.LogInformation($"Command executed successfully {inner.GetType().FullName}");

            return response;
        }
    }
}