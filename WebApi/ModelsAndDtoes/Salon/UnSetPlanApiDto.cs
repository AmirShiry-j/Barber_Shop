using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Salon
{
    public class UnSetPlanApiDto
    {
        [Required]
        public int TimeModeId { get; set; }
        [Required]
        public string DaysOfWeek { get; set; }
        public string? Excepts { get; set; }
    }
}
