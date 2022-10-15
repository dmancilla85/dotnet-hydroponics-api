using System.ComponentModel.DataAnnotations;

namespace Hydroponics.Controllers.Requests
{
  internal class CultivationMethodRequest
  {
    
    public CultivationMethodRequest() 
    {
      this.Name = "";
    }

    /// <summary>
    /// Cultivation method name
    /// </summary>
    [Required]
    public string Name { get; set; }
  }
}
