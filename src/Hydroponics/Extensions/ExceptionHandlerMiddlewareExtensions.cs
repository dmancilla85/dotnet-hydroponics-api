using Hydroponics.Api.Infrastructure.Middlewares;

namespace Hydroponics.Extensions;

internal static class ExceptionHandlerMiddlewareExtensions
{
    public static void UseResilienceMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
