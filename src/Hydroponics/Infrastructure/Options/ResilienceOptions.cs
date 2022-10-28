namespace Hydroponics.Api.Infrastructure.Options;

/// <summary>
/// CircuitBreaker and Retry configuration
/// </summary>
public class ResilienceOptions
{
    /// <summary>
    /// Section name
    /// </summary>
    public const string SectionName = "Resilience";

    /// <summary>
    /// Number of exceptions before the circuit breaks
    /// </summary>
    public int ExceptionsBeforeBreak { get; set; }

    /// <summary>
    /// Exponential increase in waiting time for retries
    /// </summary>
    public int ExponentialIncreaseWaitTime { get; set; }

    /// <summary>
    /// Number of times to retry to invoke a service
    /// </summary>
    public int Retries { get; set; }

    /// <summary>
    /// Waiting time after circuit breaker
    /// </summary>
    public int SecondsToWaitAfterCut { get; set; }
}
