using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydroponics.Model;

/// <summary>
/// A pot
/// </summary>
[Table("Pot", Schema = "dbo")]
public class Pot
{
  /// <summary>
  /// Default constructor
  /// </summary>
  public Pot()
  {
    this.Name = "";
    this.Status = true;
  }

  /// <summary>
  /// Database ID
  /// </summary>
  [Key]
  public int Id { get; set; }

  /// <summary>
  /// Pot optional name
  /// </summary>
  [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
  public string Name { get; set; }

  /// <summary>
  /// Current status (active = true)
  /// </summary>
  public bool Status { get; set; }

  /// <summary>
  /// Pot height (m)
  /// </summary>
  public decimal Height { get; set; }

  /// <summary>
  /// Pot width (m)
  /// </summary>
  public decimal Width { get; set; }

  /// <summary>
  /// Pot length (m)
  /// </summary>
  public decimal Length { get; set; }

  /// <summary>
  /// Pot capacity in liters
  /// </summary>
  public decimal Liters { get; set; }

  /// <summary>
  /// Pot area (m2)
  /// </summary>
  public decimal Area { get; }

  /// <summary>
  /// Pot volume (m3)
  /// </summary>
  public decimal Volume { get; }
}