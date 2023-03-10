using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Barber
{
    public class CreateBarberApiDto
    {
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        [Required]
        [RegularExpression("(09)[0-9]{9}")]
        public string PhoneNumber { get; set; }
        [Required]
        public int SalonId { get; set; }
    }
}
