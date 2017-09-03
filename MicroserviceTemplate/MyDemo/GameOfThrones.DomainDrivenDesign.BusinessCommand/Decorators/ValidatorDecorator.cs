using DDD.Domain.Core.Exceptions;
using DDD.EventSourcing.Core.Commands;
using DDD.EventSourcing.Core.Events;
using FluentValidation;
using System.Linq;
using System.Threading.Tasks;

namespace MicroserviceArchitecture.GameOfThrones.BusinessCommand.Decorators
{
    public class ValidatorDecorator<TRequest, TResponse>
        : ICommandHandler<TRequest, TResponse>
         where TRequest : Command<TResponse>
        where TResponse : CommandResponse
    {
        private readonly ICommandHandler<TRequest, TResponse> inner;
        private readonly IValidator<TRequest>[] validators;

        public ValidatorDecorator(
            ICommandHandler<TRequest, TResponse> inner,
            IValidator<TRequest>[] validators)
        {
            this.inner = inner;
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest message)
        {
            var failures = validators
                .Select(v => v.Validate(message))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                throw new DomainException(
                    $"Command Validation Errors for type {typeof(TRequest).Name}", new ValidationException("Validation exception", failures));
            }

            var response = await inner.Handle(message);

            return response;
        }
    }
}