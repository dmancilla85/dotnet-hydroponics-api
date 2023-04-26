using System.ComponentModel.DataAnnotations;

namespace Hydroponics.Api.Data.DataTransferObjects
{
    /// <summary>
    /// Pot DTO
    /// </summary>
    public class EditPot
    {
        private const double MaximalCapacityLiters = 100;
        private const double MaximalSizeMeters = 1.0;
        private const double MinimalCapacityLiters = 0.5;
        private const double MinimalSizeMeters = 0.1;

        /// <summary>
        /// Pot height (m)
        /// </summary>
        /// <example>0.30</example>
        [Required]
        [Range(MinimalSizeMeters, MaximalSizeMeters)]
        public decimal Height { get; set; }

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
        public string Name { get; set; } = "";

        /// <summary>
        /// Current status (active = true)
        /// </summary>
        /// <example>true</example>
        public bool Status { get; set; } = true;

        /// <summary>
        /// Pot width (m)
        /// </summary>
        /// <example>0.18</example>
        [Required]
        [Range(MinimalSizeMeters, MaximalSizeMeters)]
        public decimal Width { get; set; }
    }
}
