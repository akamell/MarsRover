using System.ComponentModel.DataAnnotations;


namespace MarsRoverTracking.MarsRover.Domain
{
    public class MovementRequest
    {
        [Required]
        [StringLength(10, MinimumLength = 1)]
        public string RoverId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string MovementInstruction { get; set; }
    }
}