using FluentValidation;
using Hydroponics.Api.Data.DataTransferObjects;

namespace Hydroponics.Validators;

internal class UpdatePotValidator : AbstractValidator<EditPot>
{
    public UpdatePotValidator()
    {
        RuleFor(o => o.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(o => o.Liters).NotNull().GreaterThan(0.5m).LessThan(100m);
        RuleFor(o => o.Height).NotNull().NotEmpty().GreaterThan(0.1m).LessThan(1m);
        RuleFor(o => o.Width).NotNull().NotEmpty().GreaterThan(0.1m).LessThan(1m);
        RuleFor(o => o.Length).NotNull().NotEmpty().GreaterThan(0.1m).LessThan(1m);
    }
}
