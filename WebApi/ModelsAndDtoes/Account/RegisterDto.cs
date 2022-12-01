using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Account
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(1, 2)]
        public Gender Gender { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }
    }
    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
