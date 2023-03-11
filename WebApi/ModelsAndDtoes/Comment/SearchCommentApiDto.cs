using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Comment
{
    public class SearchCommentApiDto
    {
        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
        [Required]
        public int SalonId { get; set; }
    }
}
