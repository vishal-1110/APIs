using APIs.Models;
using FluentValidation;

namespace APIs.Validators
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.CustomerEmail)
                .NotEmpty().WithMessage("Customer email is mandatory.")
                .EmailAddress().WithMessage("Enter a valid email address.");

            RuleFor(x => x.ItemName)
                .NotEmpty().WithMessage("Item name cannot be blank.")
                .MaximumLength(40).WithMessage("Item name must be under 40 characters.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        }
    }
}
