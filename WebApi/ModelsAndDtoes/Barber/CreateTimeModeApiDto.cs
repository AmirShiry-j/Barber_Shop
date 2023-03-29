using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Barber
{
    public class CreateTimeModeApiDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }

}
