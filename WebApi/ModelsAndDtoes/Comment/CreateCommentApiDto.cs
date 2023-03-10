using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Comment
{
    public class CreateCommentApiDto
    {
        [Required]
        [MaxLength(100)]
        public string Text { get; set; }
        [Required]
        public int BarberId { get; set; }
        [Required]
        [Range(0,2)]
        public SuggestionModeApiDto SuggestionMode { get; set; }
    }
    public enum SuggestionModeApiDto
    {
        UnSuggested = 0,//پیشنهاد نمیشود
        Neutral = 1,//نظری ندارد
        Suggested = 2,//پیشنهاد میشود
    }
}
