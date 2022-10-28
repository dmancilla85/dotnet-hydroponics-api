using FluentValidation;
using Hydroponics.Api.Data.DataTransferObjects;
using Hydroponics.Validators;

namespace Hydroponics.Extensions;

internal static class FluentValidationExtensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<NewCultivationMethod>, CreateCultivationMethodValidator>();
        services.AddScoped<IValidator<NewMeasure>, CreateMeasureValidator>();
        services.AddScoped<IValidator<NewSubstrate>, CreateSubstrateValidator>();
        services.AddScoped<IValidator<NewPot>, CreatePotValidator>();
        services.AddScoped<IValidator<EditCultivationMethod>, UpdateCultivationMethodValidator>();
        services.AddScoped<IValidator<EditMeasure>, UpdateMeasureValidator>();
        services.AddScoped<IValidator<EditSubstrate>, UpdateSubstrateValidator>();
        services.AddScoped<IValidator<EditPot>, UpdatePotValidator>();
    }
}
