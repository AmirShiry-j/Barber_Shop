using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Salon
{
    public class SetPlanApiDto
    {
        [Required]
        public int TimeModeId { get; set; }
        [Required]
        //[RegularExpression("(09)[0-9]{9}|\\*")]
        public string DaysOfWeek { get; set; }
        public string? Excepts { get; set; }
    }
}
