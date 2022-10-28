namespace Hydroponics.Api.Data.DataTransferObjects;

/// <summary>
/// Growing medium
/// </summary>
public class ViewSubstrate
{
    /// <summary>
    /// Database id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Substrate name
    /// </summary>
    public string? Name { get; set; }
}