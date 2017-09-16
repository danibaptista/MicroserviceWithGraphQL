using FluentValidation;

namespace MicroserviceArchitecture.GameOfThrones.Domain.WriteService.Validations
{
    using DDD.EventSourcing.Core.Commands;
    using MicroserviceArchitecture.GameOfThrones.Domain.WriteModel;

    public class IdentifierCommandValidator : AbstractValidator<IdentifiedCommand<CreateOrderCommand, CommandResponse>>
    {
        public IdentifierCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}