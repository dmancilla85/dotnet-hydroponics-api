namespace Hydroponics.Api.Infrastructure.Services;

/// <summary>
/// Resilience service
/// </summary>
public interface IResilienceService
{
    /// <summary>
    /// Applies the circuit breaker pattern
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public Task CircuitBreaker(Func<Task> action);

    /// <summary>
    /// Applies the retry pattern inside a circuit breaker
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public Task CircuitBreakerWithRetry(Func<Task> action);

    /// <summary>
    /// Applies the retry pattern inside a circuit breaker and returns a value
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    Task<TResult> CircuitBreakerWithRetry<TResult>(Func<Task<TResult>> action);

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public string GetCircuitBreakerState();

    /// <summary>
    /// Applies the retry pattern
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public Task Retry(Func<Task> action);

    /// <summary>
    /// Applies the circuit breaker pattern inside a retry pattern
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public Task RetryWithCircuitBreaker(Func<Task> action);
}
