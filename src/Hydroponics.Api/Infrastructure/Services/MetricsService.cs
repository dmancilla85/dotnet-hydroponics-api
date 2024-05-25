using Prometheus;

namespace Hydroponics.Api.Infrastructure.Services;

/// <summary>
/// Prometheus metrics manager
/// </summary>
internal class MetricsService : IMetricsService
{
    private static readonly Counter ErrorsCounter = Metrics
            .CreateCounter("api_errors_total", "Total errors on execution", "error");

    private static readonly Counter EventsCounter = Metrics
            .CreateCounter("api_events_total", "Total events on execution", "event");

    private static readonly Gauge ServiceDownGauge = Metrics
            .CreateGauge("api_service_down", "Service down indicator");

    private static readonly Counter WarningsCounter = Metrics
                                            .CreateCounter("api_warnings_total", "Total warnings on execution", "warning");

    public void CountError(string label)
    {
        if (label != null)
        {
            ErrorsCounter.WithLabels(label)
                    .Inc();
        }
    }

    public void CountEvent(string label)
    {
        if (label != null)
        {
            EventsCounter.WithLabels(label)
                    .Inc();
        }
    }

    public void CountWarning(string label)
    {
        if (label != null)
        {
            WarningsCounter.WithLabels(label)
                    .Inc();
        }
    }

    public void ReportServiceDown(bool down)
    {
        ServiceDownGauge.Set(down ? 1 : 0);
    }
}
