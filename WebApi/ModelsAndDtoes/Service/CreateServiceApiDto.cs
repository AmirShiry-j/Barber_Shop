using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Service
{
    public class CreateServiceApiDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
