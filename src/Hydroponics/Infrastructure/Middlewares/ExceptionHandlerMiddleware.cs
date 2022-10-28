using System.Text.Json;
using Hydroponics.Api.Infrastructure.ErrorHandling;
using Hydroponics.Api.Infrastructure.Logging;
using Hydroponics.Api.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;

namespace Hydroponics.Api.Infrastructure.Middlewares;

internal class ExceptionHandlerMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly IMetricsService _metricsService;
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger, IMetricsService metricsService)
    {
        _next = next;
        _logger = logger;
        _metricsService = metricsService;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
        }
    }

    private Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
    {
        _ = exception ?? throw new ArgumentNullException(nameof(exception));

        ProblemDetails problem;
        LogWriter.LogExceptionHandled(_logger, context.Request.Method, context.Request.Path, exception.Message, exception.InnerException?.Message ?? "");
        _metricsService.CountError(exception.GetType().Name);

        if (exception is BrokenCircuitException)
        {
            problem = new()
            {
                Type = ProblemDetailsType.ServiceUnavailable,
                Title = exception.GetType().Name,
                Detail = exception.Message,
                Instance = context.Request.Path,
                Status = StatusCodes.Status503ServiceUnavailable
            };

            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        }
        else
        {
            problem = new()
            {
                Type = ProblemDetailsType.InternalServerError,
                Title = exception.GetType().Name,
                Detail = exception.Message,
                Instance = context.Request.Path,
                Status = StatusCodes.Status500InternalServerError
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        problem.Extensions.Add("source", exception.Source);

        context.Response.ContentType = "application/problem+json";
        return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
