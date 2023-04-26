namespace Hydroponics.Api.Data.DataTransferObjects;

/// <summary>
/// A pot
/// </summary>
public class ViewPot
{
    /// <summary>
    /// Pot area (m2)
    /// </summary>
    /// <example>0.5</example>
    public decimal Area { get; set; }

    /// <summary>
    /// Pot height (m)
    /// </summary>
    /// <example>0.30</example>
    public decimal Height { get; set; }

    /// <summary>
    /// Database ID
    /// </summary>
    /// <example>6</example>
    public int Id { get; set; }

    /// <summary>
    /// Pot length (m)
    /// </summary>
    /// <example>0.22</example>
    public decimal Length { get; set; }

    /// <summary>
    /// Pot capacity in liters
    /// </summary>
    /// <example>20</example>
    public decimal Liters { get; set; }

    /// <summary>
    /// Pot optional name
    /// </summary>
    /// <example>WASP3</example>
    public string? Name { get; set; }

    /// <summary>
    /// Current status (active = true)
    /// </summary>
    /// <example>true</example>
    public bool Status { get; set; }

    /// <summary>
    /// Pot volume (m3)
    /// </summary>
    /// <example>0.5</example>
    public decimal Volume { get; set; }

    /// <summary>
    /// Pot width (m)
    /// </summary>
    /// <example>0.18</example>
    public decimal Width { get; set; }
}