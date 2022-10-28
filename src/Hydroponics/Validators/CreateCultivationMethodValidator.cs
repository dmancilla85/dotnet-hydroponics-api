using FluentValidation;
using Hydroponics.Api.Data.DataTransferObjects;
using Hydroponics.Data;

namespace Hydroponics.Validators;

internal class CreateCultivationMethodValidator : AbstractValidator<NewCultivationMethod>
{
    public CreateCultivationMethodValidator(HydroponicsContext db)
    {
        RuleFor(o => o.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(o => o.Name).Must((o, cancellation) =>
        {
            bool exists = db.CultivationMethods.FirstOrDefault(x => string.Equals(x.Name, o.Name)) != null;
            return !exists;
        }).WithMessage($"This cultivation method name already exists.");
    }
}
