using FluentValidation;

namespace MicroserviceArchitecture.GameOfThrones.BusinessCommand.Validations
{
    using Commands;
    using DDD.EventSourcing.Core.Commands;

    public class IdentifierCommandValidator : AbstractValidator<IdentifiedCommand<CreateOrderCommand, CommandResponse>>
    {
        public IdentifierCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}