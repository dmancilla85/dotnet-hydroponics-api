using FluentValidation;
using Hydroponics.Api.Data.DataTransferObjects;
using Hydroponics.Data;

namespace Hydroponics.Validators;

internal class CreateSubstrateValidator : AbstractValidator<NewSubstrate>
{
    public CreateSubstrateValidator(HydroponicsContext db)
    {
        RuleFor(o => o.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(o => o.Name).Must((o, cancellation) =>
        {
            bool exists = db.Substrates.FirstOrDefault(x => string.Equals(x.Name, o.Name)) != null;
            return !exists;
        }).WithMessage($"This substrate name already exists.");
    }
}
