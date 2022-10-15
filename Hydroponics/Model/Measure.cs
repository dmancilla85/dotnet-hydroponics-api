using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydroponics.Model;

/// <summary>
/// Measure types
/// </summary>
[Table("Measure", Schema = "dbo")]
public class Measure
{
  /// <summary>
  ///  Default constructor
  /// </summary>
  public Measure()
  {
    this.Name = "";
    this.Description = "";
    this.Units = "";
  }

  /// <summary>
  /// Database ID
  /// </summary>
  [Key]
  public int Id { get; set; }

  /// <summary>
  /// Measure name
  /// </summary>
  [Required]
  [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
  public string Name { get; set; }

  /// <summary>
  /// Measure description
  /// </summary>
  [Required]
  [StringLength(100, MinimumLength = 3, ErrorMessage = "Description length must be between 3 and 50 characters")]
  public string Description { get; set; }

  /// <summary>
  /// Measure units
  /// </summary>
  [Required]
  [StringLength(10, MinimumLength = 1, ErrorMessage = "Units name length must be between 3 and 50 characters")]
  public string Units { get; set; }

  /// <summary>
  /// Minimum value allowed
  /// </summary>
  public decimal MinValue { get; set; }

  /// <summary>
  /// Maximum value allowed
  /// </summary>
  public decimal MaxValue { get; set; }
}