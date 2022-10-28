using FluentValidation;

namespace Hydroponics.Api.Infrastructure.Filters;

internal class ValidationFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var parameter = context.Arguments?.SingleOrDefault(p => p?.GetType() == typeof(T));
        if (parameter is null)
            return Results.Problem("The parameter is invalid.", statusCode: StatusCodes.Status404NotFound);

        var result = await _validator.ValidateAsync((T)parameter);
        if (!result.IsValid)
        {
            var errors = result.ToDictionary();
            return Results.ValidationProblem(errors);
        }

        // now the actual endpoint execution
        return await next(context);
    }
}
