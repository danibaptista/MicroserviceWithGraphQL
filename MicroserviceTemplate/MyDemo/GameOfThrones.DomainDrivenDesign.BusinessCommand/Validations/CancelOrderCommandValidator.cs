using FluentValidation;

namespace MicroserviceArchitecture.GameOfThrones.BusinessCommand.Validations
{
    using Commands;

    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator()
        {
            RuleFor(order => order.OrderNumber).NotEmpty().WithMessage("No orderId found");
        }
    }
}