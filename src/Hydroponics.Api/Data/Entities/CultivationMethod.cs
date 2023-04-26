using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydroponics.Data.Entities;

/// <summary>
/// A cultivation method
/// </summary>
[Table("CultivationMethod", Schema = "dbo")]
public class CultivationMethod
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public CultivationMethod()
    {
        Name = "";
    }

    /// <summary>
    /// Database ID
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; private set; }

    /// <summary>
    /// Cultivation method name
    /// </summary>
    /// <example>DWC</example>
    [Required(ErrorMessage = "Cultivation method name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
    public string Name { get; set; }
}