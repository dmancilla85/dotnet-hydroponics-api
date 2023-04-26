using Hydroponics.Api.Infrastructure.Logging;
using Hydroponics.Api.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace Hydroponics.Api.Infrastructure.Services;

internal class ResilienceService : IResilienceService
{
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
    private readonly ILogger<ResilienceService> _logger;
    private readonly IMetricsService _metricsService;
    private readonly ResilienceOptions _resilienceOptions;
    private readonly AsyncRetryPolicy _retryPolicy;

    public ResilienceService(IOptions<ResilienceOptions> options, ILogger<ResilienceService> logger, IMetricsService metricsService)
    {
        _logger = logger;
        _resilienceOptions = options.Value;
        _metricsService = metricsService;

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                _resilienceOptions.Retries,
                SetUpRetry,
                onRetry: (e, span, i, ctx) => LogWriter.LogRetryOperation(_logger, i, e.Message, e.GetType().Name, span.TotalSeconds));

        _circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(_resilienceOptions.ExceptionsBeforeBreak,
                TimeSpan.FromSeconds(_resilienceOptions.SecondsToWaitAfterCut),
                (ex, t) =>
                {
                    LogWriter.LogCircuitOpen(_logger, ex.Message, t.TotalSeconds);
                    _metricsService.ReportServiceDown(true);
                },
                () =>
                {
                    LogWriter.LogCircuitClosed(_logger);
                    _metricsService.ReportServiceDown(false);
                },
                () =>
                {
                    LogWriter.LogCircuitHalfOpen(_logger);
                });
    }

    public async Task CircuitBreaker(Func<Task> action) => await _circuitBreakerPolicy
        .ExecuteAsync(action);

    public async Task CircuitBreakerWithRetry(Func<Task> action) => await _circuitBreakerPolicy
        .WrapAsync(_retryPolicy)
        .ExecuteAsync(action);

    public async Task<TResult> CircuitBreakerWithRetry<TResult>(Func<Task<TResult>> action) => await _circuitBreakerPolicy
        .WrapAsync(_retryPolicy)
        .ExecuteAsync(action);

    public string GetCircuitBreakerState() => _circuitBreakerPolicy.CircuitState.ToString();

    public async Task Retry(Func<Task> action) => await _retryPolicy
                .ExecuteAsync(action);

    public async Task RetryWithCircuitBreaker(Func<Task> action) => await _retryPolicy
        .WrapAsync(_circuitBreakerPolicy)
        .ExecuteAsync(action);

    private TimeSpan SetUpRetry(int retryAttempt)
    {
        _metricsService.ReportServiceDown(false);
        TimeSpan timeToWait = TimeSpan
                        .FromSeconds(Math.Pow(_resilienceOptions.ExponentialIncreaseWaitTime, retryAttempt));
        LogWriter.LogRetryAttempt(_logger, timeToWait.TotalSeconds);
        return timeToWait;
    }
}
