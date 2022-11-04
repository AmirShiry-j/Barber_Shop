using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Account
{
    public class ChangePasswordDto
    {
        [Required]
        [MinLength(6)]
        public string CurrentPassword { get; set; }
        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [MinLength(6)]
        [Compare(nameof(NewPassword))]
        [DataType(DataType.Password)]
        public string ReNewPassword { get; set; }
    }
}
