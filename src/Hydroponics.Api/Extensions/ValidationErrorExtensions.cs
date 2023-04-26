using FluentValidation.Results;

namespace Hydroponics.Extensions;

internal static class ValidationErrorExtensions
{
    public static string GetErrors(this List<ValidationFailure> errors)
    {
        var errorMessages = "";
        errors.ForEach(err => errorMessages += err.ErrorMessage + " ");

        return errorMessages;
    }
}
