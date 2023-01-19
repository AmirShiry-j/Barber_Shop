using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Salon
{
    public class CreateSalonApiDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int CityId { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullAddress { get; set; }
        [Required]
        [RegularExpression("[0-9]+")]
        public string Telphone { get; set; }
        [Required]
        [RegularExpression("(09)[0-9]{9}")]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
    }
}
