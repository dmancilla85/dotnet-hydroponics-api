using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hydroponics.Model
{
  [Table("CultivationMethod", Schema = "dbo")]
  public class CultivationMethod
  {
    public CultivationMethod() {
      this.Name = "";
    }

    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage= "Cultivation method name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 50 characters")]
    public string Name { get; set; }
  }
}
