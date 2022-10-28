using FluentValidation;
using Hydroponics.Api.Data.DataTransferObjects;

namespace Hydroponics.Validators;

internal class UpdateSubstrateValidator : AbstractValidator<EditSubstrate>
{
    public UpdateSubstrateValidator()
    {
        RuleFor(o => o.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(50);
    }
}
