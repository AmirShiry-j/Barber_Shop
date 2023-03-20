using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Comment
{
    public class SearchCommentApiDto
    {
        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;

        //One of the following two values ​​must have a value
        public int? SalonId { get; set; }
        public int? BarberId { get; set; }
    }
}
