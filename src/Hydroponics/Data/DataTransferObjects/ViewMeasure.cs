namespace Hydroponics.Api.Data.DataTransferObjects;

/// <summary>
/// Measure types
/// </summary>
public class ViewMeasure
{
    /// <summary>
    /// Measure description
    /// </summary>
    /// <example>potential hydrogen</example>
    public string? Description { get; set; }

    /// <summary>
    /// Database ID
    /// </summary>
    /// <example>12</example>
    public int Id { get; set; }

    /// <summary>
    /// Maximum value allowed
    /// </summary>
    /// <example>14</example>
    public decimal MaxValue { get; set; }

    /// <summary>
    /// Minimum value allowed
    /// </summary>
    /// <example>0</example>
    public decimal MinValue { get; set; }

    /// <summary>
    /// Measure name
    /// </summary>
    /// <example>pH</example>
    public string? Name { get; set; }

    /// <summary>
    /// Measure units
    /// </summary>
    /// <example>N/A</example>
    public string? Units { get; set; }
}