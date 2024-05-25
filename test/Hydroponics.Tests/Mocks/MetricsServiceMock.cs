using Hydroponics.Api.Infrastructure.Services;

namespace Hydroponics.Tests.Mocks;

internal class MetricsServiceMock : IMetricsService
{
    public void CountError(string label)
    {
        return;
    }

    public void CountEvent(string label)
    {
        return;
    }

    public void CountWarning(string label)
    {
        return;
    }

    public void ReportServiceDown(bool down)
    {
        return;
    }
}
