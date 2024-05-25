using System.ComponentModel.DataAnnotations;

namespace Hydroponics.Api.Data.DataTransferObjects;

/// <summary>
/// cultivation method DTO
/// </summary>
public class NewCultivationMethod
{
    /// <summary>
    /// Cultivation method name
    /// </summary>
    /// <example>DWC</example>
    [Required(ErrorMessage = "Cultivation method name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
    public string Name { get; set; } = "";
}
