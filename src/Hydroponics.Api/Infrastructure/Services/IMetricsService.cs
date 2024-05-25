namespace Hydroponics.Api.Infrastructure.Services;

/// <summary>
/// Prometheus metrics manager
/// </summary>
public interface IMetricsService
{
    /// <summary>
    /// Contabilizar un error específico.
    /// </summary>
    /// <remarks>La cantidad de labels distintos por métrica no debe ser mayor a 50.</remarks>
    /// <param name="label">Tipo de error</param>
    void CountError(string label);

    /// <summary>
    /// Contabilizar un evento específico
    /// </summary>
    /// <param name="label">Tipo de evento</param>
    void CountEvent(string label);

    /// <summary>
    /// Contabilizar un warning específico
    /// </summary>
    /// <param name="label">Tipo de warning</param>
    void CountWarning(string label);

    /// <summary>
    /// Indicador de servicio caído --prototipo
    /// </summary>
    /// <param name="down"></param>
    void ReportServiceDown(bool down);
}
