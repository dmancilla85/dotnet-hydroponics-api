using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydroponics.Data.Entities;

/// <summary>
/// Measure types
/// </summary>
[Table("Measure", Schema = "dbo")]
public class Measure
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public Measure()
    {
        Name = "-";
        Description = "-";
        Units = "-";
    }

    /// <summary>
    /// Measure description
    /// </summary>
    /// <example>potential hydrogen</example>
    [Required(ErrorMessage = "Description is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Description length must be between 3 and 100 characters")]
    public string Description { get; set; }

    /// <summary>
    /// Database ID
    /// </summary>
    /// <example>12</example>
    [Key]
    public int Id { get; private set; }

    /// <summary>
    /// Maximum value allowed
    /// </summary>
    /// <example>14</example>
    [Required(ErrorMessage = "Maximum value is required")]
    public decimal MaxValue { get; set; }

    /// <summary>
    /// Minimum value allowed
    /// </summary>
    /// <example>0</example>
    [Required(ErrorMessage = "Minimum value is required")]
    public decimal MinValue { get; set; }

    /// <summary>
    /// Measure name
    /// </summary>
    /// <example>pH</example>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name length must be between 2 and 50 characters")]
    public string Name { get; set; }

    /// <summary>
    /// Measure units
    /// </summary>
    /// <example>N/A</example>
    [Required(ErrorMessage = "Units type is required")]
    [StringLength(10, ErrorMessage = "Units name length must be 10 characters as maximum")]
    public string Units { get; set; }
}
