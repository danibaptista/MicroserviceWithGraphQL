using FluentValidation;

namespace MicroserviceArchitecture.GameOfThrones.Domain.WriteService.Validations
{
    using MicroserviceArchitecture.GameOfThrones.Domain.WriteModel;

    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator()
        {
            RuleFor(order => order.OrderNumber).NotEmpty().WithMessage("No orderId found");
        }
    }
}