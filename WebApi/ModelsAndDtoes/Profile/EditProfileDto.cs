using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WebApi.ModelsAndDtoes.Account;

namespace WebApi.ModelsAndDtoes.Profile
{
    public class EditProfileDto
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [Range(1, 2)]
        public Gender Gender { get; set; }

        [RegularExpression("(09)[0-9]{9}")]
        public string? PhoneNumber { get; set; }
    }
}
