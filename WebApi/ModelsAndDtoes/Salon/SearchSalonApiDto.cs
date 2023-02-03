using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Salon
{
    public class SearchSalonApiDto
    {
        public string? SalonName { get; set; }
        public int? CityId { get; set; }
        public int? UnitedId { get; set; }
        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;

        [Range(1, 2)]
        public ForGenderApi? ForGender { get; set; }
    }
}
