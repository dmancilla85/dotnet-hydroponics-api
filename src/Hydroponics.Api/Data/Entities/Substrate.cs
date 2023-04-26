using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydroponics.Data.Entities;

/// <summary>
/// Growing medium
/// </summary>
[Table("Substrate", Schema = "dbo")]
public class Substrate
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public Substrate()
    {
        Name = "";
    }

    /// <summary>
    /// Database id
    /// </summary>
    [Key]
    public int Id { get; private set; }

    /// <summary>
    /// Substrate name
    /// </summary>
    [Required(ErrorMessage = "Substrate name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
    public string Name { get; set; }
}