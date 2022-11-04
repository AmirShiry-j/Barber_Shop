using System.ComponentModel.DataAnnotations;

namespace WebApi.Areas.Admin.ModelsAndDtoes.Roles
{
    public class EditRoleDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [MaxLength(70)]
        public string Description { get; set; }
    }
}
