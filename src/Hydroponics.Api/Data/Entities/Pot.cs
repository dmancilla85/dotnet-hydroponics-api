using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydroponics.Data.Entities;

/// <summary>
/// A pot
/// </summary>
[Table("Pot", Schema = "dbo")]
public class Pot
{
    [NotMapped] private const double MaximalCapacityLiters = 100;
    [NotMapped] private const double MaximalSizeMeters = 1.0;
    [NotMapped] private const double MinimalCapacityLiters = 0.5;
    [NotMapped] private const double MinimalSizeMeters = 0.1;

    /// <summary>
    /// Default constructor
    /// </summary>
    public Pot()
    {
        Name = "";
        Status = true;
    }

    /// <summary>
    /// Pot area (m2)
    /// </summary>
    /// <example>0.5</example>
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public decimal Area { get; private set; }

    /// <summary>
    /// Pot height (m)
    /// </summary>
    /// <example>0.30</example>
    [Required]
    [Range(MinimalSizeMeters, MaximalSizeMeters)]
    public decimal Height { get; set; }

    /// <summary>
    /// Database ID
    /// </summary>
    /// <example>6</example>
    [Key]
    public int Id { get; private set; }

    /// <summary>
    /// Pot length (m)
    /// </summary>
    /// <example>0.22</example>
    [Required]
    [Range(MinimalSizeMeters, MaximalSizeMeters)]
    public decimal Length { get; set; }

    /// <summary>
    /// Pot capacity in liters
    /// </summary>
    /// <example>20</example>
    [Required]
    [Range(MinimalCapacityLiters, MaximalCapacityLiters)]
    public decimal Liters { get; set; }

    /// <summary>
    /// Pot optional name
    /// </summary>
    /// <example>WASP3</example>
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
    public string Name { get; set; }

    /// <summary>
    /// Current status (active = true)
    /// </summary>
    /// <example>true</example>
    public bool Status { get; set; }

    /// <summary>
    /// Pot volume (m3)
    /// </summary>
    /// <example>0.5</example>
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public decimal Volume { get; private set; }

    /// <summary>
    /// Pot width (m)
    /// </summary>
    /// <example>0.18</example>
    [Required]
    [Range(MinimalSizeMeters, MaximalSizeMeters)]
    public decimal Width { get; set; }
}