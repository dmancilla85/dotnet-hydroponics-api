namespace Hydroponics.Api.Infrastructure.Options;

/// <summary>
/// Configuration for rate limiter
/// </summary>
public class RateLimitingOptions
{
    /// <summary>
    /// Section name
    /// </summary>
    public const string SectionName = "RateLimiter";

    /// <summary>
    /// Maximum number of permit counters that can be allowed in a window. Must be set to a value > 0 by the time these options are passed to the constructor of FixedWindowRateLimiter.
    /// </summary>
    public int PermitLimit { get; set; }

    /// <summary>
    /// Maximum cumulative permit count of queued acquisition requests. Must be set to a value >= 0 by the time these options are passed to the constructor of FixedWindowRateLimiter.
    /// </summary>
    public int QueueLimit { get; set; }

    /// <summary>
    /// Specifies the maximum number of segments a window is divided into. Must be set to a value > 0 by the time these options are passed to the constructor of SlidingWindowRateLimiter.
    /// </summary>
    public int SegmentsPerWindow { get; set; }

    /// <summary>
    /// Specifies the time window that takes in the requests. Must be set to a value >= Zero by the time these options are passed to the constructor of FixedWindowRateLimiter.
    /// </summary>
    public int Window { get; set; }
}
