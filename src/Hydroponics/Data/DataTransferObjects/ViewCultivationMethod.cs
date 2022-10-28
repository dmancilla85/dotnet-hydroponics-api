namespace Hydroponics.Api.Data.DataTransferObjects;

/// <summary>
/// A cultivation method
/// </summary>
public class ViewCultivationMethod
{
    /// <summary>
    /// Database ID
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>
    /// Cultivation method name
    /// </summary>
    /// <example>DWC</example>
    public string? Name { get; set; }
}