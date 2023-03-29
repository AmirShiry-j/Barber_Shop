using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Barber
{
    public class CreateTimeModeItemApiDto
    {
        [Required]
        [Range(0,23)]
        public int Hour { get; set; }
        [Required]
        [Range(0,59)]
        public int Minute { get; set; }
        [Required]
        public int TimeModeId { get; set; }
    }

}
