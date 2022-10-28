using FluentValidation;
using Hydroponics.Api.Data.DataTransferObjects;

namespace Hydroponics.Validators;

internal class UpdateCultivationMethodValidator : AbstractValidator<EditCultivationMethod>
{
    public UpdateCultivationMethodValidator()
    {
        RuleFor(o => o.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(50);
    }
}
