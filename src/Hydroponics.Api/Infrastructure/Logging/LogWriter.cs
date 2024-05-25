namespace Hydroponics.Api.Infrastructure.Logging;

internal static partial class LogWriter
{
    [LoggerMessage(7, LogLevel.Warning, "{message}")]
    public static partial void LogBadRequest(ILogger logger, string message);

    [LoggerMessage(4, LogLevel.Warning, "The circuit has been restablished!")]
    public static partial void LogCircuitClosed(ILogger logger);

    [LoggerMessage(5, LogLevel.Warning, "The circuit state is half-open...")]
    public static partial void LogCircuitHalfOpen(ILogger logger);

    [LoggerMessage(3, LogLevel.Warning, "The circuit state is now open due to {message}. Time: {seconds} seconds")]
    public static partial void LogCircuitOpen(ILogger logger, string message, double seconds);

    // exceptions
    [LoggerMessage(6, LogLevel.Error, "An exception has occurred in {method} {path}: {message}. {innerMessage}")]
    public static partial void LogExceptionHandled(ILogger logger, string method, string path, string message, string innerMessage);

    [LoggerMessage(2, LogLevel.Warning, "Waiting {seconds} seconds to next try")]
    public static partial void LogRetryAttempt(ILogger logger, double seconds);

    // resilience service
    [LoggerMessage(1, LogLevel.Warning, "Retry {number}: {message} due to {exception}. Waiting {seconds} seconds.")]
    public static partial void LogRetryOperation(ILogger logger, int number, string message, string exception, double seconds);
}
