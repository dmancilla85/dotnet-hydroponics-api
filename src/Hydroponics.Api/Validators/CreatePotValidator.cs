using FluentValidation;
using Hydroponics.Api.Data.DataTransferObjects;
using Hydroponics.Data;
using Microsoft.Data.SqlClient;

namespace Hydroponics.Validators;

internal class CreatePotValidator : AbstractValidator<NewPot>
{
    public CreatePotValidator(HydroponicsContext db)
    {
        RuleFor(o => o.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(o => o.Liters).NotNull().GreaterThan(0.5m).LessThan(100m);
        RuleFor(o => o.Height).NotNull().NotEmpty().GreaterThan(0.1m).LessThan(1m);
        RuleFor(o => o.Width).NotNull().NotEmpty().GreaterThan(0.1m).LessThan(1m);
        RuleFor(o => o.Length).NotNull().NotEmpty().GreaterThan(0.1m).LessThan(1m);
        RuleFor(o => o.Name).Must((o, cancellation) =>
        {
            try
            {
                bool exists = db.Pots.FirstOrDefault(x => string.Equals(x.Name, o.Name)) != null;
                return !exists;
            }
            catch (SqlException)
            {
                return true;
            }
        }).WithMessage($"This pot name already exists.");
    }
}
