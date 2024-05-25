using System.ComponentModel.DataAnnotations;

namespace Hydroponics.Api.Data.DataTransferObjects;

/// <summary>
/// Substrate DTO
/// </summary>
public class NewSubstrate
{
    /// <summary>
    /// Substrate name
    /// </summary>
    [Required(ErrorMessage = "Substrate name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
    public string Name { get; set; } = "";
}
