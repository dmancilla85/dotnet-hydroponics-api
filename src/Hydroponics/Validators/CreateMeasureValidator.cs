using FluentValidation;
using Hydroponics.Api.Data.DataTransferObjects;
using Hydroponics.Data;

namespace Hydroponics.Validators;

internal class CreateMeasureValidator : AbstractValidator<NewMeasure>
{
    public CreateMeasureValidator(HydroponicsContext db)
    {
        RuleFor(o => o.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(o => o.Description).NotNull().NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(o => o.Units).NotNull().NotEmpty().MaximumLength(10);
        RuleFor(o => o.MinValue).NotNull();
        RuleFor(o => o.MaxValue).NotNull();

        RuleFor(o => o.Name).Must((o, cancellation) =>
            {
                bool exists = db.Measures.FirstOrDefault(x => string.Equals(x.Name, o.Name)) != null;
                return !exists;
            }).WithMessage($"This measure name already exists.");
    }
}
