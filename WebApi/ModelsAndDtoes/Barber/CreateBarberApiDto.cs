using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Barber
{
    public class CreateBarberApiDto
    {
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        [Required]
        public int SalonId { get; set; }
    }
}
