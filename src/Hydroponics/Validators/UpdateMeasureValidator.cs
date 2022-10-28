using FluentValidation;
using Hydroponics.Api.Data.DataTransferObjects;

namespace Hydroponics.Validators;

internal class UpdateMeasureValidator : AbstractValidator<EditMeasure>
{
    public UpdateMeasureValidator()
    {
        RuleFor(o => o.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(o => o.Description).NotNull().NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(o => o.Units).NotNull().NotEmpty().MaximumLength(10);
        RuleFor(o => o.MinValue).NotNull();
        RuleFor(o => o.MaxValue).NotNull();
    }
}
