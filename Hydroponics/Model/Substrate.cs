using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydroponics.Model
{
  [Table("Substrate", Schema = "dbo")]
  public class Substrate
  {
    public Substrate() {
      this.Name = "";
    }

    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage="Substrate name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
    public string Name { get; set; }
  }
}
